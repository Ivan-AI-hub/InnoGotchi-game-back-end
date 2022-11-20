using InnoGotchiGame.Application.Interfaces;
using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence
{
    public class InnoGotchiGameContext : DbContext, IInnoGotchiGameContext
    {
        public InnoGotchiGameContext()
        {
        }

        public InnoGotchiGameContext(DbContextOptions<InnoGotchiGameContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<PetFarm> PetFarms { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbSet<User> Users { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}