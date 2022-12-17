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

        public int Age => IsAlive ? (DateTime.Now - BornDate).Days : (int)(DeadDate - BornDate)?.Days;
        public int HappinessDayCount => IsAlive ? (FirstHappinessDay - DateTime.Now).Days : 0;
        public double AverageDrinkingPeriod => DrinkingCount != 0 ? Age / DrinkingCount : DrinkingCount;
        public double AverageFeedingPeriod => FeedingCount != 0 ? Age / FeedingCount : FeedingCount;
    }
}
