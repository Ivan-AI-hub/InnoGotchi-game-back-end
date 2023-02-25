using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Domain
{
    public record PetStatistic : IPetStatistic
    {
        public string Name { get; set; }

        public DateTime BornDate { get; private set; }
        public DateTime? DeadDate { get; set; }
        public bool IsAlive => DeadDate == null;

        public int FeedingCount { get; private set; }
        public int DrinkingCount { get; private set; }
        public DateTime DateLastFeed { get; private set; }
        public DateTime DateLastDrink { get; private set; }

        public DateTime FirstHappinessDay { get; private set; }

        private PetStatistic() { }
        public PetStatistic(string name)
        {
            Name = name;
            BornDate = DateTime.UtcNow;
            FirstHappinessDay = DateTime.UtcNow;
            DateLastFeed = DateTime.UtcNow;
            DateLastDrink = DateTime.UtcNow;
            FeedingCount = 1;
            DrinkingCount = 1;
        }

        public void Feed()
        {
            FeedingCount++;
            DateLastFeed = DateTime.UtcNow;
        }

        public void Drink()
        {
            DrinkingCount++;
            DateLastDrink = DateTime.UtcNow;
        }

        public void ResetFirstHappinessDay()
        {
            FirstHappinessDay = DateTime.UtcNow;
        }
    }
}
