using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PictureRepository : RepositoryBase<IPicture, Picture>, IPictureRepository
    {
        public PictureRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<IPicture> GetItems(bool trackChanges)
        {
            var pictures = Context.Pictures.AsQueryable().AsNoTracking();

            return trackChanges ? pictures : pictures.AsNoTracking();
        }
    }
}
