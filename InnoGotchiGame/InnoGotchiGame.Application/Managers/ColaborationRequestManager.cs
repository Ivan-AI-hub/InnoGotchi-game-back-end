using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Managers;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working on collaborating requests
    /// </summary>
    public class ColaborationRequestManager
    {
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<ColaborationRequest> _requestRepository;

        public ColaborationRequestManager(IRepositoryManager repositoryManager)
        {
            _requestRepository = repositoryManager.ColaborationRequest;
            _repositoryManager = repositoryManager;
        }

        /// <summary>
        /// Create a collaborating request from <paramref name="senderId"/> to <paramref name="recipientId"/>
        /// </summary>
        /// <param name="senderId">id of the sending user</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> SendColaborationRequestAsync(int senderId, int recipientId)
        {
            var result = new ManagerResult();
            var request = new ColaborationRequest() { RequestSenderId = senderId, RequestReceiverId = recipientId, Status = ColaborationRequestStatus.Undefined };

            var isSingleRequest = await _requestRepository.IsItemExistAsync(x => x.RequestSenderId == senderId && x.RequestReceiverId == recipientId ||
                                                           x.RequestReceiverId == senderId && x.RequestSenderId == recipientId);
            if (!isSingleRequest)
            {
                _requestRepository.Create(request);
                _repositoryManager.SaveAsync().Wait();
            }
            else
            {
                result.Errors.Add("The collaborating request for these users already exists");
            }

            return result;
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> ConfirmRequestAsync(int requestId, int recipientId)
        {
            var result = new ManagerResult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request != null)
            {
                if (request.Status == ColaborationRequestStatus.Colaborators)
                    result.Errors.Add("Request already confirmed");
                if (request.RequestReceiverId != recipientId)
                    result.Errors.Add("Only the recipient of the request can confirm the request. The recipient's ID does not match");
                if (result.IsComplete)
                {
                    request.Status = ColaborationRequestStatus.Colaborators;
                    _requestRepository.Update(request);
                    _repositoryManager.SaveAsync().Wait();
                }
            }
            else
            {
                result.Errors.Add("The request ID is not in the database");
            }

            return result;
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="participantId">id of the participant</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> RejectRequestAsync(int requestId, int participantId)
        {
            var result = new ManagerResult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request != null)
            {
                if (request.Status == ColaborationRequestStatus.NotColaborators)
                    result.Errors.Add("Request already rejected");
                if (request.RequestReceiverId != participantId && request.RequestSenderId != participantId)
                    result.Errors.Add("Only the participant of the request can reject the request. The recipient's ID does not match");

                if (result.IsComplete)
                {
                    request.Status = ColaborationRequestStatus.NotColaborators;
                    _requestRepository.Update(request);
                    _repositoryManager.SaveAsync().Wait();
                }
            }
            else
            {
                result.Errors.Add("The request ID is not in the database");
            }

            return result;
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteRequestAsync(int requestId)
        {
            var result = new ManagerResult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request != null)
            {
                _requestRepository.Delete(request);
                _repositoryManager.SaveAsync().Wait();
            }
            else
            {
                result.Errors.Add("The request ID is not in the database");
            }

            return result;
        }
    }
}
