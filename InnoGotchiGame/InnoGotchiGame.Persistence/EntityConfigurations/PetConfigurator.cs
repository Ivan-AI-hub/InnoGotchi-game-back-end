using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class PetConfigurator : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.OwnsOne(p => p.View, view =>
            {
                view.HasOne(x => x.Picture).WithMany().OnDelete(DeleteBehavior.SetNull);
            });
            builder.OwnsOne(p => p.Statistic, cb =>
            {
                cb.HasIndex(x => x.Name).IsUnique();
            });

            builder.HasOne(d => d.Farm)
                .WithMany(p => p.Pets)
                .HasForeignKey(d => d.FarmId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
