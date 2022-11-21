using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class PetFarmConfigurator : IEntityTypeConfiguration<PetFarm>
    {
        public void Configure(EntityTypeBuilder<PetFarm> builder)
        {
            builder.Ignore(x => x.AlivesPetsCount);
            builder.Ignore(x => x.DeadsPetsCount);
            builder.Ignore(x => x.AveragePetsHappinessDaysCount);
            builder.Ignore(x => x.AveragePetsAge);

            builder.HasOne(p => p.User)
                .WithOne(d => d.OwnPetFarm)
                .HasForeignKey<User>(x => x.OwnPetFarmId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
