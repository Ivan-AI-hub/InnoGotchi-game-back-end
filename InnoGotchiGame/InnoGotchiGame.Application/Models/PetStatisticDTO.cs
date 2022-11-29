namespace InnoGotchiGame.Application.Models
{
	public record PetStatisticDTO
	{
		public string? Name { get; set; }
		public DateTime BornDate { get; set; }
		public bool IsAlive { get; set; }
		public int FeedingCount { get; set; }
		public int DrinkingCount { get; set; }
		public DateTime FirstHappinessDay { get; set; }
		public DateTime DateLastFeed { get; set; }
		public DateTime DateLastDrink { get; set; }
	}
}
