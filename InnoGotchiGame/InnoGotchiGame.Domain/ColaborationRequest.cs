using InnoGotchiGame.Domain.enums;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Domain
{
    public class ColaborationRequest : IColaborationRequest
    {
        public int Id { get; private set; }
        public ColaborationRequestStatus Status { get; set; }

        public int RequestSenderId { get; private set; }
        public IUser? RequestSender { get; private set; }

        public int RequestReceiverId { get; private set; }
        public IUser? RequestReceiver { get; private set; }

        private ColaborationRequest() { }
        private ColaborationRequest(ColaborationRequestStatus status)
        {
            Status = status;
        }
        public ColaborationRequest(IUser requestSender, IUser requestReceiver, ColaborationRequestStatus status) : this(status)
        {
            RequestSender = requestSender;
            RequestSenderId = requestSender.Id;
            RequestReceiver = requestReceiver;
            RequestReceiverId = requestReceiver.Id;
        }
        public ColaborationRequest(int requestSenderId, int requestReceiverId, ColaborationRequestStatus status) : this(status)
        {
            RequestSenderId = requestSenderId;
            RequestReceiverId = requestReceiverId;
        }
    }
}
