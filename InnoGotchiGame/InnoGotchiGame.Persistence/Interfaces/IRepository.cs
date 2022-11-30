namespace InnoGotchiGame.Persistence.Interfaces
{
	public interface IRepository<T> where T : class
	{
		public T? GetItemById(int id);
		public bool IsItemExist(int id);

		public T? GetItem(Func<T, bool> predicate);


		public IQueryable<T> GetItems(Func<T, bool>? whereRule = null,
										 Func<T, dynamic>? orderByRule = null,
										 bool isDescendingOrder = false);

		public int Add(T item);

		public void Update(int updatedId, T item);

		public bool Delete(int id);

		public void Save();
	}
}
