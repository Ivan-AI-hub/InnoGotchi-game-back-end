namespace InnoGotchiGame.Domain
{
    public class ColaborationRequest
    {
        public int Id { get; set; }
        public ColaborationRequestStatus Status { get; set; }

        public int RequestSenderId { get; set; }
        public User RequestSender { get; set; }

        public int RequestReceiverId { get; set; }
        public User RequestReceiver { get; set; }
    }
}
