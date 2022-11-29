using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Sorters
{
	public class UserSorter : Sorter<User>
	{
		public bool IsFirstNameSort { get; set; }
		public bool IsLastNameSort { get; set; }
		public bool IsEmailSort { get; set; }
		public bool IsDescendingSort { get; set; }

		public UserSorter(bool isFirstNameSort = true, bool isLastNameSort = false, bool isEmailSort = false)
		{
			IsFirstNameSort = isFirstNameSort;
			IsLastNameSort = isLastNameSort;
			IsEmailSort = isEmailSort;
			IsDescendingSort = false;
		}



		internal override IQueryable<User> Sort(IQueryable<User> users)
		{
			if (!IsDescendingSort)
			{
				if (IsFirstNameSort)
					return users.OrderBy(x => x.FirstName);
				else if (IsLastNameSort)
					return users.OrderBy(x => x.LastName);
				else if (IsEmailSort)
					return users.OrderBy(x => x.Email);
			}
			else
			{
				if (IsFirstNameSort)
					return users.OrderByDescending(x => x.FirstName);
				else if (IsLastNameSort)
					return users.OrderByDescending(x => x.LastName);
				else if (IsEmailSort)
					return users.OrderByDescending(x => x.Email);
			}
			return users;
		}
	}
}
