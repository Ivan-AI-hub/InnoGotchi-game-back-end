
namespace InnoGotchiGame.Domain
{
    public class PetFarm
    {
        public int Id { get; set; }

        public int FeedingPeriod { get; set; }
        public int QuenchingPeriod { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Pet> Pets { get; }
    }
}
