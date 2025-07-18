using LoLEsportsHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.Data.Common.EntityConstants.TournamentMatch;
namespace LoLEsportsHub.Data.Configuration
{
    public class TournamentMatchConfiguration : IEntityTypeConfiguration<TournamentMatch>
    {
        public void Configure(EntityTypeBuilder<TournamentMatch> entity)
        {
            entity
                .HasKey(tm => tm.Id);

            entity
                .HasIndex(cm => new { cm.MatchId, cm.TournamentId, cm.ScheduledTime })
                .IsUnique(true);

            entity
                .Property(tm => tm.ScheduledTime)
                .IsRequired()
                .HasMaxLength(ScheduledTimeMaxLength);

            entity
                .Property(tm => tm.AvailableSlots)
                .HasDefaultValue(AvailableSlotsDefaultValue);

            entity
                .Property(cm => cm.IsDeleted)
                .HasDefaultValue(false);

            entity.Property(tm => tm.AvailableSlots)
                .IsRequired();

            entity
                .HasQueryFilter(cm => cm.IsDeleted == false &&
                                                cm.Tournament.IsDeleted == false &&
                                                cm.Match.IsDeleted == false);

            entity
                .HasMany(tm => tm.Tickets)
                .WithOne(t => t.TournamentMatch)
                .HasForeignKey(t => t.TournamentMatchId);
            
            entity
               .HasOne(tm => tm.Tournament)
               .WithMany(m => m.Matches)
               .HasForeignKey(cm => cm.TournamentId)
               .OnDelete(DeleteBehavior.Restrict);

            entity
              .HasOne(tm => tm.Match)
              .WithMany(m => m.Tournaments)
              .HasForeignKey(cm => cm.TournamentId)
              .OnDelete(DeleteBehavior.Restrict);


            //entity
                //.HasData(this.SeedGamesMatches());
        }

        private IEnumerable<TournamentMatch> SeedGamesMatches()
        {
            List<TournamentMatch> gameMatches = new List<TournamentMatch>()
            {
                new TournamentMatch()
    {
                Id = Guid.Parse("fa2be4a6-c170-4ec4-b8c1-f4bc6fd8ee7f"),
                TournamentId = Guid.Parse("f6c156e3-a5b7-4cf9-98c2-6577b8b6fa61"), // MSI 2025
                MatchId = Guid.Parse("a1d6219f-9f49-4f29-a2ea-d3c302781b01"),     // G2 vs Fnatic
                ScheduledTime = "2025-05-02 18:00",
                IsDeleted = false
            },
            new TournamentMatch()
            {
                Id = Guid.Parse("e3a83cb3-df47-492e-b4f0-4a3cfe9d3a6c"),
                TournamentId = Guid.Parse("bcb29915-27de-45ea-8c29-0e4c8ff6d703"), // Worlds
                MatchId = Guid.Parse("b2e732af-82f2-4ab2-9cd5-47f2a2ffcf32"),      // T1 vs Gen.G
                ScheduledTime = "2025-10-10 20:00",
                IsDeleted = false
            }
            };
            return gameMatches;
        }
    }
}
