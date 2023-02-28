using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class UserConfigurator : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasKey(user => user.Id);
            builder.HasOne(p => (PetFarm) p.OwnPetFarm)
                .WithOne(d => (User)d.Owner)
                .HasForeignKey<PetFarm>(x => x.OwnerId);

            builder.HasOne(user => (Picture) user.Picture).WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
