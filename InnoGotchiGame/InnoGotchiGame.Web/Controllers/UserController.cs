﻿using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Sorters;

namespace InnoGotchiGame.Web.Controllers
{
	[Route("api/users")]
	public class UserController : BaseController
	{
		private UserManager _manager;

		public UserController(UserManager manager)
		{
			_manager = manager;
		}

		[HttpPost]
		public IActionResult Post(string firstName, string lastName, string email, string password)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest();
			}

			UserDTO user = new UserDTO() { FirstName = firstName, LastName = lastName, Email = email, Password = password };
			
			var rezult = _manager.Add(user);
			if (!rezult.IsComplete)
				return BadRequest();

			return Ok(user);
		}

		[HttpPut]
		public IActionResult Put(int userId, string firstName, string lastName, string email, string password = "")
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			UserDTO user = new UserDTO() { FirstName = firstName, LastName = lastName, Email = email, Password = password };

			var rezult = _manager.Update(userId, user);

			if (!rezult.IsComplete)
				return BadRequest();

			return Ok(user);
		}

		[HttpGet]
		public IActionResult Get(UserFiltrator? filtrator = null, UserSorter? sorter = null)
		{
			var users = _manager.GetUsers(filtrator, sorter);
			return new ObjectResult(users);
		}

		[HttpGet("{userId}")]
		public IActionResult GetById(int userId)
		{
			var user = _manager.GetUserById(userId);
			if(user == null)
				return BadRequest(new { errorText = "Invalid id." });
			
			return new ObjectResult(user);
		}

		[HttpPost("api/users/token")]
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
			UserDTO? user = _manager.FindUserInDb(email, password);
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