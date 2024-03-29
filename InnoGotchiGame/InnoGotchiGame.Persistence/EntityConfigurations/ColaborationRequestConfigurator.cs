﻿using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class ColaborationRequestConfigurator : IEntityTypeConfiguration<ColaborationRequest>
    {
        public void Configure(EntityTypeBuilder<ColaborationRequest> builder)
        {
            builder.HasOne(d => (User)d.RequestSender)
                .WithMany(p => (IEnumerable<ColaborationRequest>)p.SentColaborations)
                .HasForeignKey(d => d.RequestSenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => (User)d.RequestReceiver)
                .WithMany(p => (IEnumerable<ColaborationRequest>)p.AcceptedColaborations)
                .HasForeignKey(d => d.RequestReceiverId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
