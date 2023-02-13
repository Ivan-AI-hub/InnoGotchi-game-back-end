using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Models.ErrorModel;
using InnoGotchiGame.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly UserManager _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(UserManager manager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = manager;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="addUserModel"></param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> PostAsync([FromBody] AddUserModel addUserModel)
        {
            UserDTO user = _mapper.Map<UserDTO>(addUserModel);

            var result = await _userManager.AddAsync(user, addUserModel.Password);
            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Updates a User data
        /// </summary>
        /// <param name="updateUserModel"></param>
        [HttpPut("data")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> PutDataAsync([FromBody] UpdateUserDataModel updateUserModel)
        {
            UserDTO user = _mapper.Map<UserDTO>(updateUserModel);

            var result = await _userManager.UpdateDataAsync(updateUserModel.UpdatedId, user);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Accepted();
        }

        /// <summary>
        /// Updates a User data
        /// </summary>
        /// <param name="updateUserModel"></param>
        [HttpPut("password")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> PutPasswordAsync([FromBody] UpdateUserPasswordModel updateUserModel)
        {
            var result = await _userManager.UpdatePasswordAsync(updateUserModel.UpdatedId, updateUserModel.OldPassword, updateUserModel.NewPassword);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Accepted();
        }

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> DeleteAsync(int userId)
        {
            var result = await _userManager.DeleteAsync(userId);
            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return NoContent();
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> users</returns>
        [HttpGet("{pageSize}/{pageNumber}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<IActionResult> GetPageAsync(int pageSize, int pageNumber, UserFiltrator filtrator,
                                    string sortField = "LastName", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var users = await _userManager.GetUsersPageAsync(pageSize, pageNumber, filtrator, sorter);
            return Ok(users);
        }

        /// <returns>Filtered and sorted list of users</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public async Task<IActionResult> GetAsync(UserFiltrator filtrator, string sortField = "LastName", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var users = await _userManager.GetUsersAsync(filtrator, sorter);
            return Ok(users);
        }

        /// <param name="userId"></param>
        /// <returns>a user with same Id</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 404)]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound(new ErrorDetails(404, "Invalid id."));

            return new ObjectResult(user);
        }

        /// <returns>Authorized user</returns>
        [HttpGet("Authorized")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        public async Task<IActionResult> GetAuthorizeUserAsync()
        {
            var userId = int.Parse(User.GetUserId()!);
            var user = await _userManager.GetUserByIdAsync(userId);

            return new ObjectResult(user);
        }

        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>access token for user witn same email and password</returns>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> TokenAsync(string email, string password)
        {
            UserDTO? user = await _userManager.FindUserInDbAsync(email, password);
            if (user == null)
            {
                return BadRequest(new ErrorDetails(400, "Invalid email or password."));
            }

            var token = GetToken(user);
            return Json(token);
        }

        private UserSorter GetSorter(string sortRule, bool isDescendingSort)
        {
            var sorter = new UserSorter();
            sorter.SortRule = Enum.Parse<UserSortRule>(sortRule);
            sorter.IsDescendingSort = isDescendingSort;
            return sorter;
        }

        private AuthorizeModel GetToken(UserDTO user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(nameof(TokenClaims.UserId), user.Id.ToString())
            };

            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("issuerSigningKey").Value);

            var jwt = new JwtSecurityToken(
                    issuer: jwtSettings.GetSection("validIssuer").Value,
                    audience: jwtSettings.GetSection("validAudience").Value,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            var token = new AuthorizeModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                User = user
            };
            return token;
        }
    }
}
