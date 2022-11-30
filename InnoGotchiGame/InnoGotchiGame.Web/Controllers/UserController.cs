using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using Microsoft.AspNetCore.Mvc;

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
		public IActionResult Post(UserDTO user)
		{
			if (user == null)
				return BadRequest();

			var rezult = _manager.Add(user);
			if (!rezult.IsComplete)
				return BadRequest();

			return Ok(user);
		}
	}
}
