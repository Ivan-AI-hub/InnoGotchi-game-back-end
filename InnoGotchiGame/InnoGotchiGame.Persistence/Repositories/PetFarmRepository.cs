using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PetFarmRepository : RepositoryBase<IPetFarm, PetFarm>, IPetFarmRepository
    {
        public PetFarmRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<IPetFarm> GetItems(bool trackChanges)
        {
            var petFarms = Context.PetFarms
                        .Include(x => x.Owner)
                        .Include(x => x.Pets)
                            .ThenInclude(x => x.View.Picture);


            return trackChanges ? petFarms : petFarms.AsNoTracking();
        }
    }
}
