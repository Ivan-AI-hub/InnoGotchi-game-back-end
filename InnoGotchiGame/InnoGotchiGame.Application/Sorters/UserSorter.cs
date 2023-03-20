using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Application.Sorters
{
    public class UserSorter : Sorter<IUser>
    {
        public UserSortRule SortRule { get; set; }

        protected override IQueryable<IUser> AscendingOrder(IQueryable<IUser> query)
        {
            switch (SortRule)
            {
                case UserSortRule.FirstName: return query.OrderBy(x => x.FirstName);
                case UserSortRule.LastName: return query.OrderBy(x => x.LastName);
                case UserSortRule.Email: return query.OrderBy(x => x.Email);
                default: throw new NotImplementedException();
            }
        }

        protected override IQueryable<IUser> DescendingOrder(IQueryable<IUser> query)
        {
            switch (SortRule)
            {
                case UserSortRule.FirstName: return query.OrderByDescending(x => x.FirstName);
                case UserSortRule.LastName: return query.OrderByDescending(x => x.LastName);
                case UserSortRule.Email: return query.OrderByDescending(x => x.Email);
                default: throw new NotImplementedException();
            }
        }
    }
}
