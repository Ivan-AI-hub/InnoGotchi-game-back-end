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
		public double AverageFeedingPeriod => Pets.Average(x => x.Statistic.AverageFeedingPeriod);
		public double AverageDrinkingPeriod => Pets.Average(x => x.Statistic.AverageDrinkingPeriod);
		public double AveragePetsHappinessDaysCount => Pets.Average(x => x.Statistic.HappinessDayCount);
		public double AveragePetsAge => Pets.Average(x => x.Statistic.Age);
		public int FeedingCount => Pets.Sum(x => x.Statistic.FeedingCount);
		public int DrinkingCount => Pets.Sum(x => x.Statistic.DrinkingCount);
	}
}
