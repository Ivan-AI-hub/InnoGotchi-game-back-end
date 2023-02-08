using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PictureRepository : RepositoryBase<Picture>, IPictureRepository
    {
        public PictureRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<Picture> GetItems(bool trackChanges)
        {
            var pictures = Context.Pictures.AsQueryable().AsNoTracking();

            return trackChanges ? pictures : pictures.AsNoTracking();
        }
    }
}
