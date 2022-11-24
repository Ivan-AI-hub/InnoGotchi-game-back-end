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

		public int FeedingPeriod { get; set; }
		public int QuenchingPeriod { get; set; }

		public int UserId { get; set; }
		public UserDTO User { get; set; }

		public List<PetDTO> Pets { get; }

		public int AlivesPetsCount => Pets.Count(x => x.Statistic.IsAlive);
		public int DeadsPetsCount => Pets.Count(x => !x.Statistic.IsAlive);
		public double AveragePetsHappinessDaysCount => Pets.Average(x => (x.Statistic.FirstHappinessDay - DateTime.Now).Days);
		public double AveragePetsAge => Pets.Average(x => x.Statistic.Age);
	}
}
