namespace InnoGotchiGame.Persistence.Interfaces
{
	public interface IRepository<T> where T : class
	{
		/// <returns>item from database with special id</returns>
		public T? GetItemById(int id);

		/// <returns>true if the element exists, and false if not</returns>
		public bool IsItemExist(int id);

		/// <param name="predicate">Special predicate for element search</param>
		/// <returns>The element, if it was found in the database or null</returns>
		public T? GetItem(Func<T, bool> predicate);

		/// <returns>queryable items from the database</returns>
		public IQueryable<T> GetItems();

		/// <summary>
		/// Add item in database
		/// </summary>
		/// <returns>ID of the added element</returns>
		public int Add(T item);

		/// <summary>
		/// Update item in database
		/// </summary>
		public void Update(int updatedId, T item);

		/// <summary>
		/// Delete item from database
		/// </summary>
		/// <returns>true if complite and false if not</returns>
		public bool Delete(int id);

		/// <summary>
		/// Save changes in database
		/// </summary>
		public void Save();
	}
}
