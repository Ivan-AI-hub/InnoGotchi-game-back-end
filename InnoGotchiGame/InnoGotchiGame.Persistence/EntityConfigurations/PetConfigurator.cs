using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class PetConfigurator : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.OwnsOne(p => (PetView) p.View, view =>
            {
                view.HasOne(x => (Picture) x.Picture).WithMany().OnDelete(DeleteBehavior.SetNull);
            });
            builder.OwnsOne(p => (PetStatistic) p.Statistic, cb =>
            {
                cb.HasIndex(x => x.Name).IsUnique();
            });

            builder.HasOne(d => (PetFarm) d.Farm)
                .WithMany(p => (IEnumerable<Pet>) p.Pets)
                .HasForeignKey(d => d.FarmId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
