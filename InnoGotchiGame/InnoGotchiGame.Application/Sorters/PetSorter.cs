using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetSorter : Sorter<IPet>
    {
        public PetSortRule SortRule { get; set; }

        protected override IQueryable<IPet> AscendingOrder(IQueryable<IPet> query)
        {
            switch (SortRule)
            {
                case PetSortRule.Age: return query.OrderBy(x => x.Statistic.BornDate);
                case PetSortRule.Drink: return query.OrderBy(x => x.Statistic.DateLastDrink);
                case PetSortRule.Feeding: return query.OrderBy(x => x.Statistic.DateLastFeed);
                case PetSortRule.happinessDays: return query.OrderBy(x => x.Statistic.FirstHappinessDay);
                default: throw new NotImplementedException();
            }
        }

        protected override IQueryable<IPet> DescendingOrder(IQueryable<IPet> query)
        {
            switch (SortRule)
            {
                case PetSortRule.Age: return query.OrderByDescending(x => x.Statistic.BornDate);
                case PetSortRule.Drink: return query.OrderByDescending(x => x.Statistic.DateLastDrink);
                case PetSortRule.Feeding: return query.OrderByDescending(x => x.Statistic.DateLastFeed);
                case PetSortRule.happinessDays: return query.OrderByDescending(x => x.Statistic.FirstHappinessDay);
                default: throw new NotImplementedException();
            }
        }
    }
}
