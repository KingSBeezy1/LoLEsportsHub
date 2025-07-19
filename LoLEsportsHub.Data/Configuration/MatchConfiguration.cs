using LoLEsportsHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.Data.Common.EntityConstants.Match;

namespace LoLEsportsHub.Data.Configuration
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> entity)
        {
            entity
                .HasKey(m => m.Id);

            entity
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLength);

            entity
                .Property(m => m.Region)
                .IsRequired()
                .HasMaxLength(RegionMaxLength);

            entity
                .Property(m => m.IsDeleted)
                .HasDefaultValue(false);

            entity
                .Property(m => m.Winner)
                .IsRequired(false);

            entity
                .Property(m => m.VODUrl)
                .IsRequired(false)
                .HasMaxLength(VodUrlMaxLength);

            entity
                .HasMany(m => m.BookmarkedByUsers)
                .WithOne(b => b.Match)
                .HasForeignKey(b => b.MatchId);

            entity
                .HasMany(m => m.Tournaments)
                .WithOne(tm => tm.Match)
                .HasForeignKey(tm => tm.MatchId);

            entity
                .Property(m => m.MatchDate)
                .IsRequired();

            entity
                .HasQueryFilter(m => m.IsDeleted == false);

            entity
                .HasData(this.SeedMatches());
        }

        private IEnumerable<Match> SeedMatches()
        {
            List<Match> matches = new List<Match>()
            {
                new Match()
            {
                Id = Guid.Parse("a1d6219f-9f49-4f29-a2ea-d3c302781b01"),
                Title = "G2 vs Fnatic - Spring Finals",
                Region = "EU",
                IsDeleted = false
            },
            new Match()
            {
                Id = Guid.Parse("b2e732af-82f2-4ab2-9cd5-47f2a2ffcf32"),
                Title = "T1 vs Gen.G - LCK Clash",
                Region = "KR",
                IsDeleted = false
            },
            new Match()
            {
                Id = Guid.Parse("c3f843cb-14c6-412e-9912-3290a28f31c5"),
                Title = "Cloud9 vs TL - LCS Championship",
                Region = "NA",
                IsDeleted = false
            },
            new Match()
            {
                Id = Guid.Parse("d4a9347d-b5b1-4014-9d92-4a2f9dfc1a94"),
                Title = "PSG Talon vs Beyond - PCS Showdown",
                Region = "PCS",
                IsDeleted = false
            },
            new Match()
            {
                Id = Guid.Parse("e5b0453a-6d9c-48e3-a83d-f5ab2db2b906"),
                Title = "GAM vs Team Whales - VCS Grand Finals",
                Region = "VCS",
                IsDeleted = false
            }
            };
            return matches;
        }
    }
}
