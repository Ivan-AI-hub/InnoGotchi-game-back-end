using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PictureRepository : IRepository<Picture>
    {
        private InnoGotchiGameContext _context;
        public PictureRepository(InnoGotchiGameContext context)
        {
            _context = context;
        }
        public int Add(Picture item)
        {
            return _context.Pictures.Add(item).Entity.Id;
        }

        public bool Delete(int id)
        {
            var picture = _context.Pictures.FirstOrDefault(x => x.Id == id);
            _context.ChangeTracker.Clear();
            if (picture != null)
            {
                _context.Pictures.Remove(picture);
                return true;
            }
            return false;
        }

        public Picture? GetItem(Func<Picture, bool> predicate)
        {
            return GetItems().FirstOrDefault(predicate);
        }

        public Picture? GetItemById(int id)
        {
            return GetItems().FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Picture> GetItems()
        {
            return _context.Pictures.AsQueryable().AsNoTracking();
        }

        public bool IsItemExist(int id)
        {
            return IsItemExist(x => x.Id == id);
        }

        public bool IsItemExist(Func<Picture, bool> func)
        {
            return _context.Pictures.Any(func);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(int updatedId, Picture item)
        {
            item.Id = updatedId;
            _context.ChangeTracker.Clear();
            _context.Pictures.Update(item);
        }
    }
}
