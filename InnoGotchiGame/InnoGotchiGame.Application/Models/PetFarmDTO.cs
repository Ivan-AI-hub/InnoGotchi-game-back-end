using InnoGotchiGame.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchiGame.Application.Models
{
	public class PetFarmDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public DateTime CreateDate { get; set; }
		public int FeedingCount { get; set; }
		public int DrinkingCount { get; set; }

		public int OwnerId { get; set; }
		public User Owner { get; set; }

		public List<User> Colaborators { get; set; }
		public List<PetDTO> Pets { get; }

		public int AlivesPetsCount => Pets.Count(x => x.Statistic.IsAlive);
		public int DeadsPetsCount => Pets.Count(x => !x.Statistic.IsAlive);

		public double AverageFeedingPeriod => (CreateDate - DateTime.Now).Days / FeedingCount;
		public double AverageDrinkingPeriod => (CreateDate - DateTime.Now).Days / DrinkingCount;
		public double AveragePetsHappinessDaysCount => Pets.Average(x => (x.Statistic.FirstHappinessDay - DateTime.Now).Days);
		public double AveragePetsAge => Pets.Average(x => x.Statistic.Age);
	}
}
