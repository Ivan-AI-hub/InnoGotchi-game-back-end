using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
	internal class UserConfigurator : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasMany(user => user.CollaboratedFarms).WithMany(farm => farm.Colaborators);
		}
	}
}
