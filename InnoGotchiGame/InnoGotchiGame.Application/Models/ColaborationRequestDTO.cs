using System.Text.Json.Serialization;

namespace InnoGotchiGame.Application.Models
{
    public class ColaborationRequestDTO
    {
        public int Id { get; set; }
        public ColaborationRequestStatusDTO Status { get; set; }

        public int RequestSenderId { get; set; }
        [JsonIgnore]
        public UserDTO RequestSender { get; set; }

        public int RequestReceiverId { get; set; }
        [JsonIgnore]
        public UserDTO RequestReceiver { get; set; }
    }
}
