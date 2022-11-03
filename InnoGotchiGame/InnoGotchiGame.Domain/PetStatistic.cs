namespace InnoGotchiGame.Domain
{
    public record PetStatistic
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public int HappinessDaysCount { get; set; }
        public DateOnly DataLastFeed { get; set; }
        public DateOnly DataLastDrink { get; set; }
    }
}
