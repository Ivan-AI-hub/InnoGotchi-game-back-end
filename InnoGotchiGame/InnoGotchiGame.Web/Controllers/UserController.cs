using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Web.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace InnoGotchiGame.Web.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
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
		public IActionResult Post([FromBody]AddUserModel addUserModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			UserDTO user = _mapper.Map<UserDTO>(addUserModel);

			var rezult = _userManager.Add(user);
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
		public IActionResult PutData([FromBody]UpdateUserDataModel updateUserModel)
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
		[HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDTO>), 200)]
		public IActionResult Get(UserFiltrator? filtrator = null, UserSorter? sorter = null)
		{
			var a = User;
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
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JsonResult(new JwtSecurityTokenHandler().WriteToken(jwt));
		}
	}
}
