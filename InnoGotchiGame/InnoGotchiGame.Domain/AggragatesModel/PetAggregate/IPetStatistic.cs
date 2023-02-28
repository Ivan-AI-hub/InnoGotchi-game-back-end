namespace InnoGotchiGame.Domain.AggragatesModel.PetAggregate
{
    public interface IPetStatistic
    {
        string Name { get; set; }

        DateTime BornDate { get; }
        DateTime? DeadDate { get; set; }
        bool IsAlive { get; }

        DateTime DateLastDrink { get; }
        DateTime DateLastFeed { get; }
        int DrinkingCount { get; }
        int FeedingCount { get; }

        DateTime FirstHappinessDay { get; }

        void Feed();
        void Drink();
        void ResetFirstHappinessDay();
    }
}