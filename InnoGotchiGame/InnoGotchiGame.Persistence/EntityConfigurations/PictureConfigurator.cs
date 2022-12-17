using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class PictureConfigurator : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
        }
    }
}
