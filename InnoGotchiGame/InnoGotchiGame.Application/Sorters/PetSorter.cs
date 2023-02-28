using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetSorter : Sorter<IPet>
    {
        public PetSortRule SortRule { get; set; }

        internal override IQueryable<IPet> Sort(IQueryable<IPet> pets)
        {
            if (!IsDescendingSort)
            {
                switch(SortRule)
                {
                    case PetSortRule.Age: return pets.OrderBy(x => x.Statistic.BornDate);
                    case PetSortRule.Drink: return pets.OrderBy(x => x.Statistic.DateLastDrink);
                    case PetSortRule.Feeding: return pets.OrderBy(x => x.Statistic.DateLastFeed);
                    case PetSortRule.happinessDays: return pets.OrderBy(x => x.Statistic.FirstHappinessDay);
                    default: throw new NotImplementedException();
                }
            }

            switch (SortRule)
            {
                case PetSortRule.Age: return pets.OrderByDescending(x => x.Statistic.BornDate);
                case PetSortRule.Drink: return pets.OrderByDescending(x => x.Statistic.DateLastDrink);
                case PetSortRule.Feeding: return pets.OrderByDescending(x => x.Statistic.DateLastFeed);
                case PetSortRule.happinessDays: return pets.OrderByDescending(x => x.Statistic.FirstHappinessDay);
                default: throw new NotImplementedException();
            }
        }
    }
}
