namespace LoLEsportsHub.Data.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Region { get; set; } = null!; 
        public DateTime MatchDate { get; set; }
        public string? Winner { get; set; }
        public string? VODUrl { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ApplicationUserMatch> BookmarkedByUsers { get; set; }
            = new HashSet<ApplicationUserMatch>();
        public virtual ICollection<TournamentMatch> Tournaments { get; set; }
            = new HashSet<TournamentMatch>();
    }
}
