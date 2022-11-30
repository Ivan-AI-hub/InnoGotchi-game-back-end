using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Sorters
{
    public class PetSorter : Sorter<Pet>
	{
		public bool IsAgeSort { get; set; }
		public bool IsDrinkSort { get; set; }
		public bool IsFeedingSort { get; set; }
		public bool IsDescendingSort { get; set; }

		public PetSorter(bool isAgeSort, bool isDrinkSort, bool isFeedingSort, bool isDescendingSort)
		{
			IsAgeSort = isAgeSort;
			IsDrinkSort = isDrinkSort;
			IsFeedingSort = isFeedingSort;
			IsDescendingSort = isDescendingSort;
		}

		internal override IQueryable<Pet> Sort(IQueryable<Pet> pets)
		{
			if (!IsDescendingSort)
			{
				if (IsAgeSort)
					return pets.OrderBy(x => x.Statistic.BornDate);
				else if (IsDrinkSort)
					return pets.OrderBy(x => x.Statistic.DateLastDrink);
				else if (IsFeedingSort)
					return pets.OrderBy(x => x.Statistic.DateLastFeed);
			}
			else
			{
				if (IsAgeSort)
					return pets.OrderByDescending(x => x.Statistic.BornDate);
				else if (IsDrinkSort)
					return pets.OrderByDescending(x => x.Statistic.DateLastDrink);
				else if (IsFeedingSort)
					return pets.OrderByDescending(x => x.Statistic.DateLastFeed);
			}
			return pets;
		}
	}
}
