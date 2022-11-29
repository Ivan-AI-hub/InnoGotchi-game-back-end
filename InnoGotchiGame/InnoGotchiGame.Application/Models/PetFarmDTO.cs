using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Models
{
	public class PetFarmDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public DateTime CreateDate { get; set; }

		public int OwnerId { get; set; }
		public User Owner { get; set; }

		public List<User> Colaborators { get; set; }
		public List<PetDTO> Pets { get; }

		public int AlivesPetsCount => Pets.Count(x => x.Statistic.IsAlive);
		public int DeadsPetsCount => Pets.Count(x => !x.Statistic.IsAlive);
		public double AverageFeedingPeriod => FeedingCount != 0 ? (DateTime.Now - CreateDate).Days / FeedingCount : FeedingCount;
		public double AverageDrinkingPeriod => DrinkingCount != 0 ? (DateTime.Now - CreateDate).Days / DrinkingCount : DrinkingCount;
		public double AveragePetsHappinessDaysCount => Pets.Average(x => (x.Statistic.FirstHappinessDay - DateTime.Now).Days);
		public double AveragePetsAge => Pets.Average(x => (DateTime.Now - x.Statistic.BornDate).Days);
		public int FeedingCount => Pets.Sum(x => x.Statistic.FeedingCount);
		public int DrinkingCount => Pets.Sum(x => x.Statistic.DrinkingCount);
	}
}
