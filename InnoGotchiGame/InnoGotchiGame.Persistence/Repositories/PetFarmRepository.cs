using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
	internal class PetFarmRepository : IRepository<PetFarm>
	{
		private InnoGotchiGameContext _context;
		public PetFarmRepository(InnoGotchiGameContext context)
		{
			_context = context;
		}

		public PetFarm? GetItemById(int id)
		{
			return _context.PetFarms.FirstOrDefault(x => x.Id == id);
		}

		public PetFarm? GetItem(Func<PetFarm, bool> predicate)
		{
			return _context.PetFarms.FirstOrDefault(predicate);
		}

		public void Add(PetFarm item)
		{
			_context.PetFarms.Add(item);
		}

		public void Update(int updatedId, PetFarm item)
		{
			item.Id = updatedId;
			_context.PetFarms.Update(item);
		}

		public void Delete(int id)
		{
			var petFarm = GetItemById(id);
			if (petFarm != null)
			{
				_context.PetFarms.Remove(petFarm);
			}
		}

		public IQueryable<PetFarm> GetItems(Func<PetFarm, bool>? whereRule = null, Func<PetFarm, dynamic>? orderByRule = null, bool isDescendingOrder = false)
		{
			var petFarms = _context.PetFarms
				.Include(x => x.Owner)
				.Include(x => x.Pets);

			if (whereRule != null)
				petFarms.Where(whereRule);

			if (orderByRule != null)
			{
				if (isDescendingOrder)
					petFarms.OrderByDescending(orderByRule);
				else
					petFarms.OrderBy(orderByRule);
			}


			return petFarms;
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
