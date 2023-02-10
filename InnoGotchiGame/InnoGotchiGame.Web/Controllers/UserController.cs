using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Web.Models.ErrorModel;
using InnoGotchiGame.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private UserManager _userManager;
        private IMapper _mapper;

        public UserController(UserManager manager, IMapper mapper)
        {
            _userManager = manager;
            _mapper = mapper;
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

            var rezult = await _userManager.AddAsync(user, addUserModel.Password);
            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

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

            var rezult = await _userManager.UpdateDataAsync(updateUserModel.UpdatedId, user);

            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

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
            var rezult = await _userManager.UpdatePasswordAsync(updateUserModel.UpdatedId, updateUserModel.OldPassword, updateUserModel.NewPassword);

            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

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
            var rezult = await _userManager.DeleteAsync(userId);
            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

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
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> GetByIdAsync(int userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
                return BadRequest(new ErrorDetails(400, "Invalid id."));

            return new ObjectResult(user);
        }

        /// <returns>Authorized user</returns>
        [HttpGet("Authorized")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        public async Task<IActionResult> GetAuthorizeUserAsync()
        {
            var userId = GetAuthUserId();
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(nameof(TokenClaims.UserId), user.Id.ToString())
            };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var token = new AuthorizeModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                User = user
            };
            return Json(token);
        }

        private UserSorter GetSorter(string sortRule, bool isDescendingSort)
        {
            var sorter = new UserSorter();
            sorter.SortRule = Enum.Parse<UserSortRule>(sortRule);
            sorter.IsDescendingSort = isDescendingSort;
            return sorter;
        }
    }
}
