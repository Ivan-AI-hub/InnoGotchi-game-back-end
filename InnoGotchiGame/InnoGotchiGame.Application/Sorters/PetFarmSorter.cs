using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchiGame.Application.Sorters
{
	public class PetFarmSorter : Sorter<PetFarm>
	{
		public bool IsNameSort { get; set; } = true;
		public bool IsDescendingSort { get; set; } = false;

		internal override IQueryable<PetFarm> Sort(IQueryable<PetFarm> petFarms)
		{
			if (!IsDescendingSort)
			{
				if (IsNameSort)
					return petFarms.OrderBy(x => x.Name);
			}
			else
			{
				if (IsNameSort)
					return petFarms.OrderByDescending(x => x.Name);
			}
			return petFarms;
		}
	}
}
