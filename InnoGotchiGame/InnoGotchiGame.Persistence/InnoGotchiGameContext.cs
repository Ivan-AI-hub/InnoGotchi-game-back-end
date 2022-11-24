using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence
{
    public class InnoGotchiGameContext : DbContext
    {
        public InnoGotchiGameContext()
        {
            Database.EnsureCreated();
        }

        public InnoGotchiGameContext(DbContextOptions<InnoGotchiGameContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetFarm> PetFarms { get; set; }
        public DbSet<User> Users { get; set; }
		public DbSet<FriendlyRelation> FriendlyRelations { get; set; }

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
            modelBuilder.ApplyConfiguration(new PetConfigurator());
            modelBuilder.ApplyConfiguration(new PetFarmConfigurator());
            modelBuilder.ApplyConfiguration(new UserConfigurator());
            modelBuilder.ApplyConfiguration(new FriendlyRelationConfigurator());
        }
    }
}