using InnoGotchiGame.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Abstracts
{
    public abstract class RepositoryBase<TInterface, T> : IRepository<TInterface>
        where TInterface : class
        where T : class, TInterface
    {
        protected InnoGotchiGameContext Context;
        public RepositoryBase(InnoGotchiGameContext context)
        {
            Context = context;
        }

        public virtual void Create(TInterface item)
        {
            CheckCorrectType(item);

            Context.Set<T>().Add((T)item);
        }
        public void Update(TInterface item)
        {
            CheckCorrectType(item);

            Context.Set<T>().Update((T)item);
        }

        public void Delete(TInterface item)
        {
            CheckCorrectType(item);

            Context.Set<T>().Remove((T)item);
        }

        public Task<bool> IsItemExistAsync(Expression<Func<TInterface, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Context.Set<T>().AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<TInterface> GetItemsByCondition(Expression<Func<TInterface, bool>> predicate, bool trackChanges)
        {
            return GetItems(trackChanges).Where(predicate);
        }

        public abstract IQueryable<TInterface> GetItems(bool trackChanges);

        private void CheckCorrectType(TInterface item)
        {
            if (item is not T)
                throw new Exception($"The {item.GetType()} type is not supported");
        }
    }
}
