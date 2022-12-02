namespace InnoGotchiGame.Application.Models
{
	public record PetStatisticDTO
	{
		public string Name { get; set; }
		public DateTime BornDate { get; set; }
		public bool IsAlive { get; set; }
		public DateTime? DeadDate { get; set; }
		public int FeedingCount { get; set; }
		public int DrinkingCount { get; set; }
		public DateTime FirstHappinessDay { get; set; }
		public DateTime DateLastFeed { get; set; }
		public DateTime DateLastDrink { get; set; }

		public int Age => (DateTime.Now - BornDate).Days;
		public int HappinessDayCount => (FirstHappinessDay - DateTime.Now).Days;
		public double AverageDrinkingPeriod => DrinkingCount != 0 ? (DateTime.Now - BornDate).Days / DrinkingCount : DrinkingCount;
		public double AverageFeedingPeriod => FeedingCount != 0 ? (DateTime.Now - BornDate).Days / FeedingCount : FeedingCount;
	}
}
