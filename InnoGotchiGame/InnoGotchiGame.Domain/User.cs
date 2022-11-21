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
        public string? PhotoFileLink { get; set; }

        public int OwnPetFarmId { get; set; }
        public PetFarm? OwnPetFarm { get; set; }
        //Решить проблему с пользователями
        // не забыть раскоментить в UserConfigurator
        //public List<User>? Friends { get; set; }
    }
}
