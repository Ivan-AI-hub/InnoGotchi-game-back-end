using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class PetRepository : IRepository<Pet>
    {
        private InnoGotchiGameContext _context;
        public PetRepository(InnoGotchiGameContext context)
        {
            _context = context;
        }
        public Pet? GetItemById(int id)
        {
            return GetFullData().FirstOrDefault(x => x.Id == id);
        }

        public bool IsItemExist(int id)
        {
            return _context.Pets.Any(x => x.Id == id);
        }

        public Pet? GetItem(Func<Pet, bool> predicate)
        {
            return GetFullData().FirstOrDefault(predicate);
        }

        public int Add(Pet item)
        {
            return _context.Pets.Add(item).Entity.Id;
        }

        public void Update(int updatedId, Pet item)
        {
            item.Id = updatedId;
            _context.Pets.Update(item);
        }

        public bool Delete(int id)
        {
            var pet = GetItemById(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
                return true;
            }
            return false;
        }


        public IQueryable<Pet> GetItems()
        {
            var pets = GetOnlyDiscribeData();

            return pets;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private IQueryable<Pet> GetFullData()
        {
            var pets = _context.Pets
                 .Include(x => x.Farm)
                     .ThenInclude(x => x.Owner)
                 .Include(x => x.Farm.Owner.AcceptedColaborations)
                    .ThenInclude(x => x.RequestSender)
                 .Include(x => x.Farm.Owner.SentColaborations)
                    .ThenInclude(x => x.RequestReceiver)
                 .Include(x => x.View.Picture);


            return pets;
        }

        private IQueryable<Pet> GetOnlyDiscribeData()
        {
            var pets = _context.Pets
                .Include(x => x.View.Picture);


            return pets;
        }
    }
}
