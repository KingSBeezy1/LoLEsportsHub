using System.Net.Sockets;

namespace LoLEsportsHub.Data.Models
{
    public class TournamentMatch
    {
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public Guid MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public string ScheduledTime { get; set; } = null!;
        public int AvailableSlots { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();
    }
}