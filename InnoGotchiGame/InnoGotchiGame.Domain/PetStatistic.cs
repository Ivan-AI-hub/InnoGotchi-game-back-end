namespace InnoGotchiGame.Domain
{
    public record PetStatistic
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool IsAlive { get; set; }
        public DateOnly FirstHappinessDay { get; set; }
        public DateOnly DataLastFeed { get; set; }
        public DateOnly DataLastDrink { get; set; }
    }
}
