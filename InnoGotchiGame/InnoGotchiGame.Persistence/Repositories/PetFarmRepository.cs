using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PetFarmRepository : IRepository<PetFarm>
    {
        private InnoGotchiGameContext _context;
        public PetFarmRepository(InnoGotchiGameContext context)
        {
            _context = context;
        }

        public PetFarm? GetItemById(int id)
        {
            return GetItems().FirstOrDefault(x => x.Id == id);
        }
        public bool IsItemExist(int id)
        {
            return IsItemExist(x => x.Id == id);
        }

        public bool IsItemExist(Func<PetFarm, bool> func)
        {
            return _context.PetFarms.Any(func);
        }

        public PetFarm? GetItem(Func<PetFarm, bool> predicate)
        {
            return GetItems().FirstOrDefault(predicate);
        }

        public int Add(PetFarm item)
        {
            return _context.PetFarms.Add(item).Entity.Id;
        }

        public void Update(int updatedId, PetFarm item)
        {
            item.Id = updatedId;
            _context.PetFarms.Update(item);
        }

        public bool Delete(int id)
        {
            var petFarm = GetItemById(id);
            if (petFarm != null)
            {
                _context.PetFarms.Remove(petFarm);
                return true;
            }
            return false;
        }

        public IQueryable<PetFarm> GetItems()
        {
            var petFarms = _context.PetFarms
                .Include(x => x.Owner)
                .Include(x => x.Pets)
                    .ThenInclude(x => x.View.Picture);


            return petFarms;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
