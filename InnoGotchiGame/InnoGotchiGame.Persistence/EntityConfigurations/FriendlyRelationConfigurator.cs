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
                .WithMany(p => p.SentColaborations.OfType<ColaborationRequest>())
                .HasForeignKey(d => d.RequestSenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.RequestReceiver)
                .WithMany(p => p.AcceptedColaborations.OfType<ColaborationRequest>())
                .HasForeignKey(d => d.RequestReceiverId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
