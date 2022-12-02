using InnoGotchiGame.Application.Managers;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
	public class ColaborationRequestController : BaseController
	{
		private ColaborationRequestManager _requestManager;

		public ColaborationRequestController(ColaborationRequestManager requestManager)
		{
			_requestManager = requestManager;
		}

		[HttpPost]
		public IActionResult AddCollaborator(int senderId, int receipientId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var rezult = _requestManager.SendColaborationRequest(senderId, receipientId);
			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}
	}
}
