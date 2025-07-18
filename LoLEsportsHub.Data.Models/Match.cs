namespace LoLEsportsHub.Data.Models
{
    public class Match
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Region { get; set; } = null!; 
        public DateTime MatchDate { get; set; }
        public string Winner { get; set; } = null!;
        public string? VODUrl { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserMatchBookmark> BookmarkedByUsers { get; set; }
            = new HashSet<UserMatchBookmark>();
        public virtual ICollection<TournamentMatch> Tournaments { get; set; }
            = new HashSet<TournamentMatch>();
    }
}
