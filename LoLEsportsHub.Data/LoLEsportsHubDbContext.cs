using LoLEsportsHub.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data
{
    public class LoLEsportsHubDbContext : IdentityDbContext
    {
        public LoLEsportsHubDbContext(DbContextOptions<LoLEsportsHubDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<ApplicationUserMatch> ApplicationUserMatches { get; set; } = null!;
        public virtual DbSet<Match> Matches { get; set; } = null!;
        public virtual DbSet<Organizer> Organizers { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<Tournament> Tournaments { get; set; } = null!;
        public virtual DbSet<TournamentMatch> TournamentsMatches { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
