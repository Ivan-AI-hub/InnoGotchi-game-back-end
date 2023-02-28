using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PetFiltrator : Filtrator<IPet>
    {
        public string Name { get; set; } = "";
        public int DaysAlive { get; set; } = -1;
        public int MaxDaysFromLastFeeding { get; set; } = -1;
        public int MinDaysFromLastFeeding { get; set; } = -1;
        public int MaxDaysFromLastDrinking { get; set; } = -1;
        public int MinDaysFromLastDrinking { get; set; } = -1;


        internal override IQueryable<IPet> Filter(IQueryable<IPet> pets)
        {
            pets = pets.Where(x => x.Statistic.Name.Contains(Name));

            if (DaysAlive != -1)
            {
                var filtrationBornDate = GetDate(DaysAlive);
                pets = pets.Where(x => x.Statistic.BornDate < filtrationBornDate);
            }
            if (MinDaysFromLastFeeding != -1)
            {
                var minDateFromLastFeeding = GetDate(MinDaysFromLastFeeding);
                pets = pets.Where(x => x.Statistic.DateLastFeed <= minDateFromLastFeeding);
            }
            if (MaxDaysFromLastFeeding != -1)
            {
                var maxDateFromLastFeeding = GetDate(MaxDaysFromLastFeeding);
                pets = pets.Where(x => x.Statistic.DateLastFeed > maxDateFromLastFeeding);
            }
            if (MinDaysFromLastDrinking != -1)
            {
                var minDateFromLastDrinking = GetDate(MinDaysFromLastDrinking);
                pets = pets.Where(x => x.Statistic.DateLastDrink <= minDateFromLastDrinking);
            }
            if (MaxDaysFromLastDrinking != -1)
            {
                var maxDateFromLastDrinking = GetDate(MaxDaysFromLastDrinking);
                pets = pets.Where(x => x.Statistic.DateLastDrink > maxDateFromLastDrinking);
            }
            return pets;
        }

        private DateTime GetDate(int daysAgo)
        {
            return (DateTime.UtcNow - new TimeSpan(daysAgo, 0, 0, 0)).Date;
        }
    }
}
