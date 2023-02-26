using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Application.Sorters
{
    public class UserSorter : Sorter<IUser>
    {
        public UserSortRule SortRule { get; set; }


        internal override IQueryable<IUser> Sort(IQueryable<IUser> users)
        {
            if (!IsDescendingSort)
            {
                if (SortRule == UserSortRule.FirstName)
                    return users.OrderBy(x => x.FirstName);
                else if (SortRule == UserSortRule.LastName)
                    return users.OrderBy(x => x.LastName);
                else if (SortRule == UserSortRule.Email)
                    return users.OrderBy(x => x.Email);
            }
            else
            {
                if (SortRule == UserSortRule.FirstName)
                    return users.OrderByDescending(x => x.FirstName);
                else if (SortRule == UserSortRule.LastName)
                    return users.OrderByDescending(x => x.LastName);
                else if (SortRule == UserSortRule.Email)
                    return users.OrderByDescending(x => x.Email);
            }
            return users;
        }
    }
}
