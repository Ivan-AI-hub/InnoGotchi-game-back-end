using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Web.Models.ErrorModel;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api/colaborators")]
    public class ColaborationRequestController : BaseController
    {
        private readonly ColaborationRequestManager _requestManager;

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
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> AddCollaboratorAsync(int recipientId)
        {
            int userId = GetAuthUserId();
            var rezult = await _requestManager.SendColaborationRequestAsync(userId, recipientId);
            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

            return Ok();
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>d
        [HttpPut("{requestId}/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> ConfirmRequestAsync(int requestId)
        {
            int userId = GetAuthUserId();
            var rezult = await _requestManager.ConfirmRequestAsync(requestId, userId);

            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

            return Ok();
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpPut("{requestId}/reject")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> RejectRequestAsync(int requestId)
        {
            int userId = GetAuthUserId();
            var rezult = await _requestManager.RejectRequestAsync(requestId, userId);
            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

            return Ok();
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpDelete("{requestId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> DeleteRequestAsync(int requestId)
        {
            var rezult = await _requestManager.DeleteRequestAsync(requestId);
            if (!rezult.IsComplete)
                return BadRequest(new ErrorDetails(400, rezult.Errors));

            return Ok();
        }
    }
}
