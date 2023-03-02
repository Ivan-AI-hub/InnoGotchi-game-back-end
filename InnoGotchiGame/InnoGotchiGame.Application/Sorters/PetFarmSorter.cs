using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetFarmSorter : Sorter<IPetFarm>
    {
        public PetFarmSortRule SortRule { get; set; }

        protected override IQueryable<IPetFarm> AscendingOrder(IQueryable<IPetFarm> query)
        {
            switch (SortRule)
            {
                case PetFarmSortRule.Name: return query.OrderBy(x => x.Name);
                default: throw new NotImplementedException();
            }
        }

        protected override IQueryable<IPetFarm> DescendingOrder(IQueryable<IPetFarm> query)
        {
            switch (SortRule)
            {
                case PetFarmSortRule.Name: return query.OrderByDescending(x => x.Name);
                default: throw new NotImplementedException();
            }
        }
    }
}
