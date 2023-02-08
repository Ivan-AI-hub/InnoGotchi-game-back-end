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
        public async Task<ManagerRezult> SendColaborationRequestAsync(int senderId, int recipientId)
        {
            var rezult = new ManagerRezult();
            var request = new ColaborationRequest() { RequestSenderId = senderId, RequestReceiverId = recipientId, Status = ColaborationRequestStatus.Undefined };

            var isSingleRequest = await _requestRepository.IsItemExistAsync(x => x.RequestSenderId == senderId && x.RequestReceiverId == recipientId ||
                                                           x.RequestReceiverId == senderId && x.RequestSenderId == recipientId);
            if (isSingleRequest)
            {
                _requestRepository.Create(request);
                await _repositoryManager.SaveAsync();
            }
            else
            {
                rezult.Errors.Add("The collaborating request for these users already exists");
            }

            return rezult;
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> ConfirmRequestAsync(int requestId, int recipientId)
        {
            var rezult = new ManagerRezult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request != null)
            {
                if (request.Status == ColaborationRequestStatus.Colaborators)
                    rezult.Errors.Add("Request already confirmed");
                if (request.RequestReceiverId != recipientId)
                    rezult.Errors.Add("Only the recipient of the request can confirm the request. The recipient's ID does not match");
                if (rezult.IsComplete)
                {
                    request.Status = ColaborationRequestStatus.Colaborators;
                    _requestRepository.Update(request);
                    await _repositoryManager.SaveAsync();
                }
            }
            else
            {
                rezult.Errors.Add("The request ID is not in the database");
            }

            return rezult;
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="participantId">id of the participant</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> RejectRequestAsync(int requestId, int participantId)
        {
            var rezult = new ManagerRezult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request != null)
            {
                if (request.Status == ColaborationRequestStatus.NotColaborators)
                    rezult.Errors.Add("Request already rejected");
                if (request.RequestReceiverId != participantId && request.RequestSenderId != participantId)
                    rezult.Errors.Add("Only the participant of the request can reject the request. The recipient's ID does not match");

                if (rezult.IsComplete)
                {
                    request.Status = ColaborationRequestStatus.NotColaborators;
                    _requestRepository.Update(request);
                    await _repositoryManager.SaveAsync();
                }
            }
            else
            {
                rezult.Errors.Add("The request ID is not in the database");
            }

            return rezult;
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> DeleteRequestAsync(int requestId)
        {
            var rezult = new ManagerRezult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request != null)
            {
                _requestRepository.Delete(request);
                await _repositoryManager.SaveAsync();
            }
            else
            {
                rezult.Errors.Add("The request ID is not in the database");
            }

            return rezult;
        }
    }
}
