using InnoGotchiGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
	internal class FriendlyRelationConfigurator : IEntityTypeConfiguration<ColaborationRequest>
	{
		public void Configure(EntityTypeBuilder<ColaborationRequest> builder)
		{
			builder.HasOne(d => d.RequestSender)
				.WithMany(p => p.SentColaborations)
				.HasForeignKey(d => d.RequestSenderId)
				.OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(d => d.RequestReceiver)
				.WithMany(p => p.AcceptedColaborations)
				.HasForeignKey(d => d.RequestReceiverId);
		}
	}
}
