using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Post([FromBody] AddUserModel addUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            UserDTO user = _mapper.Map<UserDTO>(addUserModel);

            var rezult = _userManager.Add(user, addUserModel.Password);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// Updates a User data
        /// </summary>
        /// <param name="updateUserModel"></param>
        [HttpPut("data")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult PutData([FromBody] UpdateUserDataModel updateUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            UserDTO user = _mapper.Map<UserDTO>(updateUserModel);

            var rezult = _userManager.UpdateData(updateUserModel.UpdatedId, user);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Accepted();
        }

        /// <summary>
        /// Updates a User data
        /// </summary>
        /// <param name="updateUserModel"></param>
        [HttpPut("password")]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult PutPassword([FromBody] UpdateUserPasswordModel updateUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _userManager.UpdatePassword(updateUserModel.UpdatedId, updateUserModel.OldPassword, updateUserModel.NewPassword);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Accepted();
        }

        /// <summary>
        /// Deletes a User
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Delete(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _userManager.Delete(userId);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return NoContent();
        }

        /// <param name="filtrator">Filtration rules</param>
        /// <param name="sorter">Sorting rules</param>
        /// <returns>All users from database</returns>
        [HttpGet("{pageSize}/{pageNumber}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult GetPage(int pageSize, int pageNumber, UserFiltrator filtrator,
                                    string sortField = "LastName", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var users = _userManager.GetUsersPage(pageSize, pageNumber, filtrator, sorter);
            return Ok(users);
        }

        /// <param name="filtrator">Filtration rules</param>
        /// <param name="sorter">Sorting rules</param>
        /// <returns>All users from database</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
        public IActionResult Get(UserFiltrator filtrator, string sortField = "LastName", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var users = _userManager.GetUsers(filtrator, sorter);
            return Ok(users);
        }

        /// <param name="userId"></param>
        /// <returns>a user with same Id</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetById(int userId)
        {
            var user = _userManager.GetUserById(userId);
            if (user == null)
                return BadRequest(new { errorText = "Invalid id." });

            return new ObjectResult(user);
        }

        /// <param name="userId"></param>
        /// <returns>a user with same Id</returns>
        [HttpGet("Authorized")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetAuthorizeUser()
        {
            var userId = int.Parse(User.FindFirstValue("Id"));
            var user = _userManager.GetUserById(userId);

            return new ObjectResult(user);
        }

        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>access token for user witn same email and password</returns>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Token(string email, string password)
        {
            UserDTO? user = _userManager.FindUserInDb(email, password);
            if (user == null)
            {
                return BadRequest(new { errorText = "Invalid email or password." });
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Email, user.Email), new Claim("Id", user.Id.ToString()) };

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
