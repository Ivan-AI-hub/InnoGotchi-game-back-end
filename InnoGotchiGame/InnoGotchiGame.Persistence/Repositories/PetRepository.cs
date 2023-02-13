using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PetRepository : RepositoryBase<Pet>, IPetRepository
    {
        public PetRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<Pet> GetItems(bool trackChanges)
        {
            return GetOnlyDiscribeData(trackChanges);
        }

        public override Task<Pet?> FirstOrDefaultAsync(Expression<Func<Pet, bool>> predicate, bool trackChanges)
        {
            return GetFullData(trackChanges).Where(predicate).FirstOrDefaultAsync();
        }

        private IQueryable<Pet> GetFullData(bool trackChanges)
        {
            var pets = Context.Pets
                 .Include(x => x.Farm)
                     .ThenInclude(x => x.Owner)
                 .Include(x => x.Farm.Owner.AcceptedColaborations)
                    .ThenInclude(x => x.RequestSender)
                 .Include(x => x.Farm.Owner.SentColaborations)
                    .ThenInclude(x => x.RequestReceiver)
                 .Include(x => x.View.Picture);


            return trackChanges ? pets : pets.AsNoTracking();
        }

        private IQueryable<Pet> GetOnlyDiscribeData(bool trackChanges)
        {
            var pets = Context.Pets
                .Include(x => x.View.Picture);
            
            return trackChanges? pets : pets.AsNoTracking();
        }
    }
}
