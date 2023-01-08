using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working on collaborating requests
    /// </summary>
    public class ColaborationRequestManager
    {
        private IRepository<ColaborationRequest> _repository;

        public ColaborationRequestManager(IRepository<ColaborationRequest> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Create a collaborating request from <paramref name="senderId"/> to <paramref name="recipientId"/>
        /// </summary>
        /// <param name="senderId">id of the sending user</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult SendColaborationRequest(int senderId, int recipientId)
        {
            var rezult = new ManagerRezult();
            var request = new ColaborationRequest() { RequestSenderId = senderId, RequestReceiverId = recipientId, Status = ColaborationRequestStatus.Undefined };

            var isSingleRequest = _repository.GetItem(x => x.RequestSenderId == senderId && x.RequestReceiverId == recipientId ||
                                                           x.RequestReceiverId == senderId && x.RequestSenderId == recipientId) == null;
            if (isSingleRequest)
            {
                _repository.Add(request);
                _repository.Save();
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
        public ManagerRezult ConfirmRequest(int requestId, int recipientId)
        {
            var rezult = new ManagerRezult();
            var request = _repository.GetItemById(requestId);
            if (request != null)
            {
                if (request.Status == ColaborationRequestStatus.Colaborators)
                    rezult.Errors.Add("Request already confirmed");
                if (request.RequestReceiverId != recipientId)
                    rezult.Errors.Add("Only the recipient of the request can confirm the request. The recipient's ID does not match");
                if (rezult.IsComplete)
                {
                    request.Status = ColaborationRequestStatus.Colaborators;
                    _repository.Update(requestId, request);
                    _repository.Save();
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
        public ManagerRezult RejectRequest(int requestId, int participantId)
        {
            var rezult = new ManagerRezult();
            var request = _repository.GetItemById(requestId);
            if (request != null)
            {
                if (request.Status == ColaborationRequestStatus.NotColaborators)
                    rezult.Errors.Add("Request already rejected");
                if (request.RequestReceiverId != participantId && request.RequestSenderId != participantId)
                    rezult.Errors.Add("Only the participant of the request can reject the request. The recipient's ID does not match");

                if (rezult.IsComplete)
                {
                    request.Status = ColaborationRequestStatus.NotColaborators;
                    _repository.Update(requestId, request);
                    _repository.Save();
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
        public ManagerRezult DeleteRequest(int requestId)
        {
            var rezult = new ManagerRezult();
            var request = _repository.GetItemById(requestId);
            if (request != null)
            {
                _repository.Delete(requestId);
                _repository.Save();
            }
            else
            {
                rezult.Errors.Add("The request ID is not in the database");
            }

            return rezult;
        }
    }
}
