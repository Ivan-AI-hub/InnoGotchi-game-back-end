using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Application.Sorters
{
    public class UserSorter : Sorter<IUser>
    {
        public UserSortRule SortRule { get; set; }


        internal override IQueryable<IUser> Sort(IQueryable<IUser> users)
        {
            if (!IsDescendingSort)
            {
                switch (SortRule)
                {
                    case UserSortRule.FirstName: return users.OrderBy(x => x.FirstName);
                    case UserSortRule.LastName: return users.OrderBy(x => x.LastName);
                    case UserSortRule.Email: return users.OrderBy(x => x.Email);
                    default: throw new NotImplementedException();
                }
            }

            switch (SortRule)
            {
                case UserSortRule.FirstName: return users.OrderByDescending(x => x.FirstName);
                case UserSortRule.LastName: return users.OrderByDescending(x => x.LastName);
                case UserSortRule.Email: return users.OrderByDescending(x => x.Email);
                default: throw new NotImplementedException();
            }

        }
    }
}
