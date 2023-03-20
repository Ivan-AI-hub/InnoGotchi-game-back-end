using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class PictureConfigurator : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
        }
    }
}
