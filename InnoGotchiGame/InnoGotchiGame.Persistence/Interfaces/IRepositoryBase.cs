using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {

        /// <returns>true if the element exists, and false if not</returns>
        public bool IsItemExist(Func<T, bool> func);

        /// <param name="predicate">Special predicate for element search</param>
        /// <returns>The element, if it was found in the database or null</returns>
        public IQueryable<T> GetItemsByCondition(Expression<Func<T, bool>> predicate, bool trackChanges);

        /// <returns>queryable items from the database</returns>
        public IQueryable<T> GetItems(bool trackChanges);

        /// <summary>
        /// Create item in database
        /// </summary>
        /// <returns>ID of the added element</returns>
        public void Create(T item);

        /// <summary>
        /// Update item in database
        /// </summary>
        public void Update(T item);

        /// <summary>
        /// Delete item from database
        /// </summary>
        /// <returns>true if complite and false if not</returns>
        public void Delete(T item);

        /// <summary>
        /// Save changes in database
        /// </summary>
        public void Save();
    }
}
