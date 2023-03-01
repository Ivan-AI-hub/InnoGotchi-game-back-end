using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchiGame.Application.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public PictureDTO? Picture { get; set; }
        public int OwnPetFarmId => OwnPetFarm != null ? OwnPetFarm.Id : 0;
        public PetFarmDTO? OwnPetFarm { get; set; }

        [JsonIgnore]
        public List<ColaborationRequestDTO> SentColaborations { get; set; }
        [JsonIgnore]
        public List<ColaborationRequestDTO> AcceptedColaborations { get; set; }
        public IEnumerable<ColaborationRequestDTO> UnconfirmedRequests => GetUnconfirmedInvites();
        public IEnumerable<ColaborationRequestDTO> RejectedRequests => GetRejectedInvites();
        public IEnumerable<UserDTO> Collaborators => GetUserColaborators();

        /// <returns>All colaborators of user</returns>
        private IEnumerable<UserDTO> GetUserColaborators()
        {
            Func<ColaborationRequestDTO, bool> whereFunc = x => x.Status == ColaborationRequestStatusDTO.Colaborators;

            List<UserDTO> friends = new List<UserDTO>();
            friends.AddRange(AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
            friends.AddRange(SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
            return friends;
        }

        /// <returns>All unconfirmed invitations to be colaborators</returns>
        private IEnumerable<ColaborationRequestDTO> GetUnconfirmedInvites()
        {
            var invites = AcceptedColaborations.Where(x => x.Status == ColaborationRequestStatusDTO.Undefined);
            return invites;
        }
        private IEnumerable<ColaborationRequestDTO> GetRejectedInvites()
        {
            var invites = SentColaborations.Where(x => x.Status == ColaborationRequestStatusDTO.NotColaborators);
            return invites;
        }
    }
}
