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

        /// <summary>
        /// Create a collaborating request from authorize user to <paramref name="recipientId"/>
        /// </summary>
        /// <param name="recipientId">id of the recipient user</param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult AddCollaborator(int recipientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            int userId = GetAuthUserId();
            var rezult = _requestManager.SendColaborationRequest(userId, recipientId);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>d
        [HttpPut("{requestId}/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult ConfirmRequest(int requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            int userId = GetAuthUserId();
            var rezult = _requestManager.ConfirmRequest(requestId, userId);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpPut("{requestId}/reject")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult RejectRequest(int requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            int userId = GetAuthUserId();
            var rezult = _requestManager.RejectRequest(requestId, userId);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpDelete("{requestId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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
