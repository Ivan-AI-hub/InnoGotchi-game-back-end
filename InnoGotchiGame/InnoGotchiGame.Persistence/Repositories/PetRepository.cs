using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PetRepository : RepositoryBase<IPet, Pet>, IPetRepository
    {
        public PetRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public IQueryable<IPet> GetFullData(bool trackChanges)
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
