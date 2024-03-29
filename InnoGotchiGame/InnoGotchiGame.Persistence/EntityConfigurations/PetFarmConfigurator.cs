﻿using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoGotchiGame.Persistence.EntityConfigurations
{
    internal class PetFarmConfigurator : IEntityTypeConfiguration<PetFarm>
    {
        public void Configure(EntityTypeBuilder<PetFarm> builder)
        {
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasOne(p => (User)p.Owner)
                .WithOne(d => (PetFarm)d.OwnPetFarm)
                .HasForeignKey<User>(x => x.OwnPetFarmId);
        }
    }
}
