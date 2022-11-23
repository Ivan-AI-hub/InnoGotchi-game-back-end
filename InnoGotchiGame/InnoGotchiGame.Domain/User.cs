using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchiGame.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PasswordHach { get; set; }

        public string? PhotoFileLink { get; set; }
        public int OwnPetFarmId { get; set; }
        public PetFarm? OwnPetFarm { get; set; }

        public List<FriendlyRelation> SentFriendships { get; set; }
        public List<FriendlyRelation> AcceptedFriendships { get; set; }
    }
}
