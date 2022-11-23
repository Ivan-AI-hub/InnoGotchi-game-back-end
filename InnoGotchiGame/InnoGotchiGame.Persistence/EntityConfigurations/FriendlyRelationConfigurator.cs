using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
	internal class FriendlyRelationConfigurator : IEntityTypeConfiguration<FriendlyRelation>
	{
		public void Configure(EntityTypeBuilder<FriendlyRelation> builder)
		{
			builder.HasOne(d => d.FirstFriend)
				.WithMany(p => p.SentFriendships)
				.HasForeignKey(d => d.FirstFriendId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(d => d.SecondFriend)
				.WithMany(p => p.AcceptedFriendships)
				.HasForeignKey(d => d.SecondFriendId);
		}
	}
}
