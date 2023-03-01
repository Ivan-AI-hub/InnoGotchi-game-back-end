using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PetRepository : RepositoryBase<IPet, Pet>, IPetRepository
    {
        public PetRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<IPet> GetItems(bool trackChanges)
        {
            return GetOnlyDiscribeData(trackChanges);
        }

        private IQueryable<IPet> GetOnlyDiscribeData(bool trackChanges)
        {
            var pets = Context.Pets
                .Include(x => x.View.Picture);

            return trackChanges ? pets : pets.AsNoTracking();
        }
    }
}
