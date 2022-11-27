using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Persistence.Interfaces
{
	public interface IRepository<T> where T:class
	{
		public T? GetItemById(int id);

		public T? GetItem(Func<T, bool> predicate);


		public IQueryable<T> GetItems(Func<T, bool>? whereRule = null,
										 Func<T, dynamic>? orderByRule = null,
										 bool isDescendingOrder = false);

		public IQueryable<T> GetItemsPage(int pageSize, int pageNumber,
										 Func<T, bool>? whereRule = null,
										 Func<T, dynamic>? orderByRule = null,
										 bool isDescendingOrder = false);

		public void Add(T item);

		public void Update(int updatedId, T item);

		public void Delete(int id);

		public void Save();
	}
}
