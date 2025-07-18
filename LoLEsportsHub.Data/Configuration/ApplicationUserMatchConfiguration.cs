using LoLEsportsHub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Configuration
{
    public class ApplicationUserMatchConfiguration : IEntityTypeConfiguration<ApplicationUserMatch>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserMatch> entity)
        {
            entity
                .HasKey(aum => new { aum.ApplicationUserId, aum.MatchId });

            entity
                .Property(aum => aum.ApplicationUserId)
                .IsRequired();

            entity
                .Property(aum => aum.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasOne(aum => aum.ApplicationUser)
                .WithMany() 
                .HasForeignKey(aum => aum.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
               .HasOne(aum => aum.Match)
               .WithMany(m => m.BookmarkedByUsers)
               .HasForeignKey(aum => aum.MatchId)
               .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasQueryFilter(aum => aum.IsDeleted == false &&
                                                        aum.Match.IsDeleted == false);
        }
    }
}
