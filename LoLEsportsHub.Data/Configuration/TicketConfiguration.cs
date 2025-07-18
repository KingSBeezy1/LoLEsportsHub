using LoLEsportsHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static LoLEsportsHub.GCommon.ApplicationConstants;

namespace LoLEsportsHub.Data.Configuration
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> entity)
        {
            entity
                .HasKey(t => t.Id);

            entity
                .Property(t => t.Price)
                .HasColumnType(PriceSqlType);

            entity
                .Property(t => t.Quantity)
                .IsRequired();

            entity
                .Property(t => t.UserId)
                .IsRequired(true);

            entity
                .HasIndex(t => new { t.TournamentMatchId, t.UserId })
                .IsUnique(true);

            entity
                .HasOne(t => t.TournamentMatch)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.TournamentMatchId);

            entity
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId);

            entity
                .HasQueryFilter(t => t.TournamentMatch.IsDeleted == false);
        }

    }
}
