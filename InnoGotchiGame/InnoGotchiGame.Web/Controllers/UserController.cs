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

		[HttpPost]
		public IActionResult Post(AddUserModel addUserModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			UserDTO user = _mapper.Map<UserDTO>(addUserModel);

			var rezult = _userManager.Add(user);
			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok(user);
		}

		[HttpPut]
		public IActionResult Put(UpdateUserModel updateUserModel)
		{
			updateUserModel.Password = updateUserModel.Password ?? "";
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			UserDTO user = _mapper.Map<UserDTO>(updateUserModel);

			var rezult = _userManager.Update(updateUserModel.UpdatedId, user);

			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}

		[HttpGet]
		public IActionResult Get(UserFiltrator? filtrator = null, UserSorter? sorter = null)
		{
			var users = _userManager.GetUsers(filtrator, sorter);
			return new ObjectResult(users);
		}

		[HttpGet("{userId}")]
		public IActionResult GetById(int userId)
		{
			var user = _userManager.GetUserById(userId);
			if (user == null)
				return BadRequest(new { errorText = "Invalid id." });

			return new ObjectResult(user);
		}

		[HttpPost("token")]
		public IActionResult Token(string email, string password)
		{
			var identity = GetIdentity(email, password);
			if (identity == null)
			{
				return BadRequest(new { errorText = "Invalid email or password." });
			}

			var now = DateTime.UtcNow;
			// создаем JWT-токен
			var jwt = new JwtSecurityToken(
					issuer: AuthOptions.ISSUER,
					audience: AuthOptions.AUDIENCE,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
					signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				access_token = encodedJwt,
				username = identity.Name
			};

			return Json(response);
		}

		private ClaimsIdentity GetIdentity(string email, string password)
		{
			UserDTO? user = _userManager.FindUserInDb(email, password);
			if (user != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
				};
				ClaimsIdentity claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
					ClaimsIdentity.DefaultRoleClaimType);
				return claimsIdentity;
			}

			return null;
		}
	}
}
