using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetFarmSorter : Sorter<IPetFarm>
    {
        public PetFarmSortRule SortRule { get; set; }

        internal override IQueryable<IPetFarm> Sort(IQueryable<IPetFarm> petFarms)
        {
            if (!IsDescendingSort)
            {
                switch(SortRule)
                {
                    case PetFarmSortRule.Name: return petFarms.OrderBy(x => x.Name);
                    default: throw new NotImplementedException();
                }    
            }

            switch (SortRule)
            {
                case PetFarmSortRule.Name: return petFarms.OrderByDescending(x => x.Name);
                default: throw new NotImplementedException();
            }
            
        }
    }
}
