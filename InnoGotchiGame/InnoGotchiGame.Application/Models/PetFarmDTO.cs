using InnoGotchiGame.Domain;
using System.Text.Json.Serialization;

namespace InnoGotchiGame.Application.Models
{
	public class PetFarmDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public DateTime CreateDate { get; set; }

		public int OwnerId { get; set; }
		[JsonIgnore]
		public UserDTO Owner { get; set; }

		public List<PetDTO> Pets { get; }

		public int AlivesPetsCount => Pets.Count(x => x.Statistic.IsAlive);
		public int DeadsPetsCount => Pets.Count(x => !x.Statistic.IsAlive);
		public double AverageFeedingPeriod => Pets.Count() != 0? Pets.Average(x => x.Statistic.AverageFeedingPeriod): 0;
		public double AverageDrinkingPeriod => Pets.Count() != 0 ? Pets.Average(x => x.Statistic.AverageDrinkingPeriod) : 0;
		public double AveragePetsHappinessDaysCount => Pets.Count() != 0 ? Pets.Average(x => x.Statistic.HappinessDayCount) : 0;
		public double AveragePetsAge => Pets.Count() != 0 ? Pets.Average(x => x.Statistic.Age) : 0;
		public int FeedingCount => Pets.Count() != 0 ? Pets.Sum(x => x.Statistic.FeedingCount) : 0;
		public int DrinkingCount => Pets.Count() != 0 ? Pets.Sum(x => x.Statistic.DrinkingCount) : 0;

		public PetFarmDTO()
		{
			Pets = new List<PetDTO>();
		}
	}
}
