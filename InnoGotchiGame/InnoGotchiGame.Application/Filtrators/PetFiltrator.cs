using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PetFiltrator : Filtrator<Pet>
    {
        public string Name { get; set; } = "";
        public int DaysAlive { get; set; } = -1;
        public int MaxDaysFromLastFeeding { get; set; } = -1;
        public int MinDaysFromLastFeeding { get; set; } = -1;
        public int MaxDaysFromLastDrinking { get; set; } = -1;
        public int MinDaysFromLastDrinking { get; set; } = -1;


        internal override IQueryable<Pet> Filter(IQueryable<Pet> query)
        {
            query = query.Where(x => x.Statistic.Name.Contains(Name));

            if (DaysAlive != -1)
            {
                var AliveDate = (DateTime.UtcNow - new TimeSpan(DaysAlive, 0, 0, 0)).Date;
                query = query.Where(x => x.Statistic.BornDate < AliveDate);
            }
            if (MinDaysFromLastFeeding != -1)
            {
                var date = GetDate(MinDaysFromLastFeeding);
                query = query.Where(x => x.Statistic.DateLastFeed <= date);
            }
            if (MaxDaysFromLastFeeding != -1)
            {
                var date = GetDate(MaxDaysFromLastFeeding);
                query = query.Where(x => x.Statistic.DateLastFeed > date);
            }
            if (MinDaysFromLastDrinking != -1)
            {
                var date = GetDate(MinDaysFromLastDrinking);
                query = query.Where(x => x.Statistic.DateLastDrink <= date);
            }
            if (MaxDaysFromLastDrinking != -1)
            {
                var date = GetDate(MaxDaysFromLastDrinking);
                query = query.Where(x => x.Statistic.DateLastDrink > date);
            }
            return query;
        }

        private DateTime GetDate(int daysAgo)
        {
            return (DateTime.UtcNow - new TimeSpan(daysAgo, 0, 0, 0)).Date;
        }
    }
}
