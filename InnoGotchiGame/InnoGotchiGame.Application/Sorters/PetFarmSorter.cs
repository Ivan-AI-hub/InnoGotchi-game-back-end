using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetFarmSorter : Sorter<PetFarm>
    {
        public PetFarmSortRule SortRule { get; set; }

        internal override IQueryable<PetFarm> Sort(IQueryable<PetFarm> petFarms)
        {
            if (!IsDescendingSort)
            {
                if (SortRule == PetFarmSortRule.Name)
                    return petFarms.OrderBy(x => x.Name);
            }
            else
            {
                if (SortRule == PetFarmSortRule.Name)
                    return petFarms.OrderByDescending(x => x.Name);
            }
            return petFarms;
        }
    }
}
