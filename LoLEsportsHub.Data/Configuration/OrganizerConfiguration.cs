using LoLEsportsHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Configuration
{
    public class OrganizerConfiguration : IEntityTypeConfiguration<Organizer>
    {
        public void Configure(EntityTypeBuilder<Organizer> entity)
        {
            entity
                .HasKey(o => o.Id);

            entity
                .Property(o => o.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(o => o.User)
                .WithOne()
                .HasForeignKey<Organizer>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasIndex(o => new { o.UserId })
                .IsUnique();

            entity
                .HasQueryFilter(m => m.IsDeleted == false);
        }
    }
}
