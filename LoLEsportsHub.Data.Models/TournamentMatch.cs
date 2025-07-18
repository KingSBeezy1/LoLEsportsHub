using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Models
{
    public class TournamentMatch
    {
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; } = null!;

        public Guid MatchId { get; set; }
        public virtual Match Match { get; set; } = null!;

        public string ScheduledTime { get; set; } = null!;
        public int AvailableSlots { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();
    }
}
