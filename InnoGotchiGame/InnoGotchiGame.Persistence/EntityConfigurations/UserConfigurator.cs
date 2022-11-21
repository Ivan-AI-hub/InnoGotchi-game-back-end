using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class UserConfigurator : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(p => p.OwnPetFarm)
                .WithOne(d => d.User)
                .HasForeignKey<PetFarm>(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(p => p.Friends).WithMany(d => d.Friends);
        }
    }
}
