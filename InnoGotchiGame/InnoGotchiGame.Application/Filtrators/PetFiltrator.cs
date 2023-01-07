using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PetFiltrator : Filtrator<Pet>
    {
        public string Name { get; set; } = "";
        public int DaysAlive { get; set; } = 0;
        public int MaxDaysFromLastFeeding { get; set; } = 100;
        public int MinDaysFromLastFeeding { get; set; } = 0;        
        public int MaxDaysFromLastDrinking { get; set; } = 100;
        public int MinDaysFromLastDrinking { get; set; } = 0;


        internal override IQueryable<Pet> Filter(IQueryable<Pet> query)
        {
            var AliveDate = (DateTime.UtcNow - new TimeSpan(DaysAlive, 0, 0, 0)).Date;
            var minFeedDate = (DateTime.UtcNow - new TimeSpan(MinDaysFromLastFeeding, 0, 0, 0)).Date;
            var maxFeedDate = (DateTime.UtcNow - new TimeSpan(MaxDaysFromLastFeeding, 0, 0, 0)).Date;            
            var minDrinkDate = (DateTime.UtcNow - new TimeSpan(MinDaysFromLastDrinking, 0, 0, 0)).Date;
            var maxDrinkDate = (DateTime.UtcNow - new TimeSpan(MaxDaysFromLastDrinking, 0, 0, 0)).Date;
            return query.Where(x => x.Statistic.Name.Contains(Name) &&
                                    x.Statistic.BornDate < AliveDate &&
                                    x.Statistic.DateLastFeed <= minFeedDate&&
                                    x.Statistic.DateLastFeed > maxFeedDate&&                                    
                                    x.Statistic.DateLastDrink <= minDrinkDate&&
                                    x.Statistic.DateLastDrink > maxDrinkDate);
        }
    }
}
