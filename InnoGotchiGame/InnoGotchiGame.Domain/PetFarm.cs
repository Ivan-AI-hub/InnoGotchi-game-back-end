
namespace InnoGotchiGame.Domain
{
    public class PetFarm
    {
        public int Id { get; set; }

        public List<Pet> Pets { get;}
        public int FeedingPeriod { get; set; }
        public int QuenchingPeriod { get; set; }

        public int AlivesPetsCount => Pets.Count(x => x.Statistic.IsAlive);
        public int DeadsPetsCount => Pets.Count(x => !x.Statistic.IsAlive);
        public double AveragePetsHappinessDaysCount => Pets.Average(x => (x.Statistic.FirstHappinessDay.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days);
        public double AveragePetsAge => Pets.Average(x => x.Statistic.Age);

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
