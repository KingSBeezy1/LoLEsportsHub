using LoLEsportsHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.Data.Common.EntityConstants.Tournament;

namespace LoLEsportsHub.Data.Configuration
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> entity)
        {
            entity
                .HasKey(t => t.Id);

            entity
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            entity
                .Property(t => t.Region)
                .IsRequired()
                .HasMaxLength(RegionMaxLength);

            entity
                .HasOne(t => t.Organizer)
                .WithMany(o => o.ManagedTournaments)
                .HasForeignKey(t => t.OrganizerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity
                .HasMany(t => t.Matches)
                .WithOne(tm => tm.Tournament)
                .HasForeignKey(tm => tm.TournamentId);

            entity
                .HasIndex(t => new { t.Name, t.Region })
                .IsUnique(true);

            entity
                .HasQueryFilter(t => t.IsDeleted == false);

            //entity
                //.HasData(SeedTournaments());

        }

        private IEnumerable<Tournament> SeedTournaments()
        {
            List<Tournament> tournaments = new List<Tournament>()
            {
                new Tournament()
    {
                Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-111111111111"),
                Name = "MSI 2025",
                Region = "Global",
                IsDeleted = false,
                OrganizerId = null // Set later if Organizer seeded
            },
            new Tournament()
            {
                Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-222222222222"),
                Name = "LEC Summer Split",
                Region = "EU",
                IsDeleted = false,
                OrganizerId = null
            },
            new Tournament()
            {
                Id = Guid.Parse("33333333-cccc-cccc-cccc-333333333333"),
                Name = "LCK Spring Playoffs",
                Region = "KR",
                IsDeleted = false,
                OrganizerId = null
            },
            new Tournament()
            {
                Id = Guid.Parse("44444444-dddd-dddd-dddd-444444444444"),
                Name = "VCS Winter Finals",
                Region = "VN",
                IsDeleted = false,
                OrganizerId = null
            }
            };
            return tournaments;
        }
    }
}
