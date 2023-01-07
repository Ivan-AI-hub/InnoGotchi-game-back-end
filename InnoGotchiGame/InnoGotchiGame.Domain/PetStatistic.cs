﻿namespace InnoGotchiGame.Domain
{
    public record PetStatistic
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

        public int GetAlivesDays()
        {
            if(DeadDate == null)
            {
                return (DateTime.UtcNow - BornDate).Days;
            }
            else
            {
                return (DeadDate - BornDate).Value.Days;
            }
        }
    }
}
