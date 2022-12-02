using InnoGotchiGame.Application.Managers;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
	[Route("/api/colaborators")]
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
		[HttpPut("confirm")]
		public IActionResult ConfirmRequest(int requestId, int receipientId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var rezult = _requestManager.ConfirmRequest(requestId, receipientId);

			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}

		[HttpPut("reqect")]
		public IActionResult RejectRequest(int requestId, int participantId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var rezult = _requestManager.RejectRequest(requestId, participantId);
			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}

		[HttpDelete]
		public IActionResult DeleteRequest(int requestId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			var rezult = _requestManager.DeleteRequest(requestId);
			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}
	}
}
