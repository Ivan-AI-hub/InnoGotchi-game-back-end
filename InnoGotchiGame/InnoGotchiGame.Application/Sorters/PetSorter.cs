using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetSorter : Sorter<IPet>
    {
        public PetSortRule SortRule { get; set; }

        internal override IQueryable<IPet> Sort(IQueryable<IPet> pets)
        {
            if (!IsDescendingSort)
            {
                if (SortRule == PetSortRule.Age)
                    return pets.OrderBy(x => x.Statistic.BornDate);
                else if (SortRule == PetSortRule.Drink)
                    return pets.OrderBy(x => x.Statistic.DateLastDrink);
                else if (SortRule == PetSortRule.Feeding)
                    return pets.OrderBy(x => x.Statistic.DateLastFeed);
                else if (SortRule == PetSortRule.happinessDays)
                    return pets.OrderBy(x => x.Statistic.FirstHappinessDay);
            }
            else
            {
                if (SortRule == PetSortRule.Age)
                    return pets.OrderByDescending(x => x.Statistic.BornDate);
                else if (SortRule == PetSortRule.Drink)
                    return pets.OrderByDescending(x => x.Statistic.DateLastDrink);
                else if (SortRule == PetSortRule.Feeding)
                    return pets.OrderByDescending(x => x.Statistic.DateLastFeed);
                else if (SortRule == PetSortRule.happinessDays)
                    return pets.OrderByDescending(x => x.Statistic.FirstHappinessDay);
            }
            return pets;
        }
    }
}
