using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
	internal class PetRepository : IRepository<Pet>
	{
		private InnoGotchiGameContext _context;
		public PetRepository(InnoGotchiGameContext context)
		{
			_context = context;
		}
		public Pet? GetItemById(int id)
		{
			return _context.Pets.FirstOrDefault(x => x.Id == id);
		}

		public Pet? GetItem(Func<Pet, bool> predicate)
		{
			return _context.Pets.FirstOrDefault(predicate);
		}

		public void Add(Pet item)
		{
			_context.Pets.Add(item);
		}

		public void Update(int updatedId, Pet item)
		{
			item.Id = updatedId;
			_context.Pets.Update(item);
		}

		public void Delete(int id)
		{
			var pet = GetItemById(id);
			if (pet != null)
			{
				_context.Pets.Remove(pet);
			}
		}


		public IQueryable<Pet> GetItems(Func<Pet, bool>? whereRule = null, Func<Pet, dynamic>? orderByRule = null, bool isDescendingOrder = false)
		{
			var pets = _context.Pets
				.Include(x => x.Farm);

			if (whereRule != null)
				pets.Where(whereRule);

			if (orderByRule != null)
			{
				if (isDescendingOrder)
					pets.OrderByDescending(orderByRule);
				else
					pets.OrderBy(orderByRule);
			}

			return pets;
		}

		public IQueryable<Pet> GetItemsPage(int pageSize, int pageNumber, Func<Pet, bool>? whereRule = null, Func<Pet, dynamic>? orderByRule = null, bool isDescendingOrder = false)
		{
			return GetItems(whereRule, orderByRule, isDescendingOrder)
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize);
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
