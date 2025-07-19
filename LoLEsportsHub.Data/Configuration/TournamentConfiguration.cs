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

            entity
                .HasData(SeedTournaments());

        }

        private IEnumerable<Tournament> SeedTournaments()
        {
            return new List<Tournament>()
            {
                new Tournament()
                {
                    Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-111111111111"),
                    Name = "MSI 2025",
                    Region = "Global",
                    IsDeleted = false,
                    OrganizerId = null
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
                    Region = "VCS",
                    IsDeleted = false,
                    OrganizerId = null
                },
                new Tournament()
                {
                    Id = Guid.Parse("55555555-eeee-eeee-eeee-555555555555"),
                    Name = "LCS Summer Championship",
                    Region = "NA",
                    IsDeleted = false,
                    OrganizerId = null
                },
                new Tournament()
                {
                    Id = Guid.Parse("66666666-ffff-ffff-ffff-666666666666"),
                    Name = "Worlds 2025",
                    Region = "Global",
                    IsDeleted = false,
                    OrganizerId = null
                },
                new Tournament()
                {
                    Id = Guid.Parse("77777777-aaaa-bbbb-cccc-777777777777"),
                    Name = "LPL Summer Playoffs",
                    Region = "LPL",
                    IsDeleted = false,
                    OrganizerId = null
                },
                new Tournament()
                {
                    Id = Guid.Parse("88888888-bbbb-cccc-dddd-888888888888"),
                    Name = "CBLOL Winter Finals",
                    Region = "CBLOL",
                    IsDeleted = false,
                    OrganizerId = null
                },
                new Tournament()
                {
                    Id = Guid.Parse("99999999-cccc-dddd-eeee-999999999999"),
                    Name = "PCS Summer Split",
                    Region = "PCS",
                    IsDeleted = false,
                    OrganizerId = null
                },
                new Tournament()
                {
                    Id = Guid.Parse("aaaaaaaa-dddd-eeee-ffff-aaaaaaaaaaaa"),
                    Name = "LLA Apertura Playoffs",
                    Region = "LLA",
                    IsDeleted = false,
                    OrganizerId = null
                }
            };
        }
    }
}
