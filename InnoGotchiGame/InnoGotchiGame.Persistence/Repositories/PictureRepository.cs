using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using System;

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
            if(picture != null)
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
            return _context.Pictures.AsQueryable();
        }

        public bool IsItemExist(int id)
        {
            return _context.Pictures.Any(x => x.Id == id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(int updatedId, Picture item)
        {
            item.Id = updatedId;
            _context.Pictures.Update(item);
        }
    }
}
