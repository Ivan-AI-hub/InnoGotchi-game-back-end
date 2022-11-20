using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Application.Interfaces
{
    public interface IInnoGotchiGameContext
    {
        DbSet<Pet> Pets { get; set; }
        DbSet<PetFarm> PetFarms { get; set; }
        DbSet<User> Users { get; set; }

        public int SaveChanges();
    }
}
