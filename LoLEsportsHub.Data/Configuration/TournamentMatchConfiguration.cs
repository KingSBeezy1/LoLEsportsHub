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
              .HasForeignKey(cm => cm.MatchId)
              .OnDelete(DeleteBehavior.Restrict);


            entity
                .HasData(this.SeedGamesMatches());
        }

        private IEnumerable<TournamentMatch> SeedGamesMatches()
        {
            return new List<TournamentMatch>()
            {
                new TournamentMatch()
                {
                    Id = Guid.Parse("fa2be4a6-c170-4ec4-b8c1-f4bc6fd8ee7f"),
                    TournamentId = Guid.Parse("11111111-aaaa-aaaa-aaaa-111111111111"), // MSI 2025
                    MatchId = Guid.Parse("a1d6219f-9f49-4f29-a2ea-d3c302781b01"),     // G2 vs Fnatic
                    ScheduledTime = "2025-05-02 18:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("e3a83cb3-df47-492e-b4f0-4a3cfe9d3a6c"),
                    TournamentId = Guid.Parse("66666666-ffff-ffff-ffff-666666666666"), // Worlds 2025
                    MatchId = Guid.Parse("b2e732af-82f2-4ab2-9cd5-47f2a2ffcf32"),      // T1 vs Gen.G
                    ScheduledTime = "2025-10-10 20:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    TournamentId = Guid.Parse("22222222-bbbb-bbbb-bbbb-222222222222"), // LEC Summer Split
                    MatchId = Guid.Parse("dd444444-4444-4444-4444-dddddddddddd"),      // MAD vs Rogue
                    ScheduledTime = "2025-07-15 17:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("22222222-3333-4444-5555-666666666666"),
                    TournamentId = Guid.Parse("33333333-cccc-cccc-cccc-333333333333"), // LCK Playoffs
                    MatchId = Guid.Parse("bb222222-2222-2222-2222-bbbbbbbbbbbb"),      // DRX vs KT
                    ScheduledTime = "2025-04-12 16:30",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("33333333-4444-5555-6666-777777777777"),
                    TournamentId = Guid.Parse("55555555-eeee-eeee-eeee-555555555555"), // LCS Championship
                    MatchId = Guid.Parse("cc333333-3333-3333-3333-cccccccccccc"),      // EG vs 100T
                    ScheduledTime = "2025-08-08 19:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("44444444-5555-6666-7777-888888888888"),
                    TournamentId = Guid.Parse("77777777-aaaa-bbbb-cccc-777777777777"), // LPL Playoffs
                    MatchId = Guid.Parse("aa111111-1111-1111-1111-aaaaaaaaaaaa"),      // TES vs RNG
                    ScheduledTime = "2025-09-01 14:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("55555555-6666-7777-8888-999999999999"),
                    TournamentId = Guid.Parse("88888888-bbbb-cccc-dddd-888888888888"), // CBLOL Finals
                    MatchId = Guid.Parse("22222222-bbbb-cccc-dddd-222222222222"),      // LOUD vs paiN
                    ScheduledTime = "2025-06-21 18:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("66666666-7777-8888-9999-aaaaaaaaaaaa"),
                    TournamentId = Guid.Parse("99999999-cccc-dddd-eeee-999999999999"), // PCS Split
                    MatchId = Guid.Parse("d4a9347d-b5b1-4014-9d92-4a2f9dfc1a94"),      // PSG vs Beyond
                    ScheduledTime = "2025-03-19 12:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("77777777-8888-9999-aaaa-bbbbbbbbbbbb"),
                    TournamentId = Guid.Parse("aaaaaaaa-dddd-eeee-ffff-aaaaaaaaaaaa"), // LLA Playoffs
                    MatchId = Guid.Parse("ff666666-6666-6666-6666-ffffffffffff"),      // Isurus vs Estral
                    ScheduledTime = "2025-05-05 21:00",
                    IsDeleted = false
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("88888888-9999-aaaa-bbbb-cccccccccccc"),
                    TournamentId = Guid.Parse("44444444-dddd-dddd-dddd-444444444444"), // VCS Finals
                    MatchId = Guid.Parse("e5b0453a-6d9c-48e3-a83d-f5ab2db2b906"),      // GAM vs Whales
                    ScheduledTime = "2025-04-02 15:30",
                    IsDeleted = false
                }
            };
        }
    }
}
