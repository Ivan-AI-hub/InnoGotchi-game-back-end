using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Abstracts
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected InnoGotchiGameContext Context;
        public RepositoryBase(InnoGotchiGameContext context) 
        {
            Context = context;
        }

        public virtual void Create(T item) => Context.Set<T>().Add(item);
        public void Update(T item) => Context.Set<T>().Update(item);
        public void Delete(T item) => Context.Set<T>().Remove(item);
        public bool IsItemExist(Expression<Func<T, bool>> predicate) => Context.Set<T>().Any(predicate);

        public IQueryable<T> GetItemsByCondition(Expression<Func<T, bool>> predicate, bool trackChanges)
        {
            return GetItems(trackChanges).Where(predicate);
        }

        public abstract IQueryable<T> GetItems(bool trackChanges);

        public virtual Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool trackChanges)
        {
            return GetItems(trackChanges).FirstOrDefaultAsync();
        }
    }
}
