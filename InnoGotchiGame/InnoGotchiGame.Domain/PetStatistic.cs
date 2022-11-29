namespace InnoGotchiGame.Domain
{
    public record PetStatistic
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool IsAlive { get; set; }
        public DateTime FirstHappinessDay { get; set; }
        public DateTime DateLastFeed { get; set; }
        public DateTime DateLastDrink { get; set; }
    }
}
