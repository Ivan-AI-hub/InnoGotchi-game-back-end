using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate
{
    public interface IColaborationRequest
    {
        int Id { get; }
        IUser RequestReceiver { get; }
        int RequestReceiverId { get; }

        IUser RequestSender { get; }
        int RequestSenderId { get; }

        ColaborationRequestStatus Status { get; set; }
    }
}