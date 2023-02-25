using InnoGotchiGame.Domain.enums;

namespace InnoGotchiGame.Domain.Interfaces
{
    public interface IColaborationRequest
    {
        int Id { get; }
        IUser RequestReceiver { get; }
        int RequestReceiverId { get; }

        IUser RequestSender { get; }
        int RequestSenderId { get; }

        ColaborationRequestStatus Status { get; }
    }
}