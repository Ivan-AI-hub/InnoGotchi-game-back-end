using System.Text.Json.Serialization;

namespace InnoGotchiGame.Application.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public PictureDTO? Picture { get; set; }

        public PetFarmDTO? OwnPetFarm { get; set; }

        [JsonIgnore]
        public List<ColaborationRequestDTO> SentColaborations { get; set; }
        [JsonIgnore]
        public List<ColaborationRequestDTO> AcceptedColaborations { get; set; }
        public IEnumerable<ColaborationRequestDTO> UnconfirmedRequest => GetUnconfirmedInvites();
        public IEnumerable<PetFarmDTO?> CollaboratedFarms => GetUserColaborators().Select(x => x.OwnPetFarm);
        public IEnumerable<int> CollaboratersId => GetUserColaborators().Select(x => x.Id);

        public UserDTO()
        {
            SentColaborations = new List<ColaborationRequestDTO>();
            AcceptedColaborations = new List<ColaborationRequestDTO>();
        }

        /// <returns>All colaborators of user</returns>
        public IEnumerable<UserDTO> GetUserColaborators()
        {
            Func<ColaborationRequestDTO, bool> whereFunc = x => x.Status == ColaborationRequestStatusDTO.Colaborators;

            List<UserDTO> friends = new List<UserDTO>();
            friends.AddRange(AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
            friends.AddRange(SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
            return friends;
        }

        /// <returns>All unconfirmed invitations to be colaborators</returns>
        public IEnumerable<ColaborationRequestDTO> GetUnconfirmedInvites()
        {
            var invites = AcceptedColaborations.Where(x => x.Status == ColaborationRequestStatusDTO.Undefined);
            return invites;
        }
    }
}
