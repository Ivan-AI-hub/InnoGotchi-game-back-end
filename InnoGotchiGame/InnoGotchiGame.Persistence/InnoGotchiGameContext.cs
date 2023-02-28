using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;
using InnoGotchiGame.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence
{
    public class InnoGotchiGameContext : DbContext
    {

        public InnoGotchiGameContext(DbContextOptions<InnoGotchiGameContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetFarm> PetFarms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ColaborationRequest> ColaborationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PetConfigurator());
            modelBuilder.ApplyConfiguration(new PetFarmConfigurator());
            modelBuilder.ApplyConfiguration(new UserConfigurator());
            modelBuilder.ApplyConfiguration(new PictureConfigurator());
            modelBuilder.ApplyConfiguration(new FriendlyRelationConfigurator());
        }
    }
}