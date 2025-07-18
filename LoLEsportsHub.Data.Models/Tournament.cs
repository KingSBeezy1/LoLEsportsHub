using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Models
{
    public class Tournament
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public Guid? OrganizerId { get; set; }
        public virtual Organizer? Organizer { get; set; }

        public virtual ICollection<TournamentMatch> Matches { get; set; } 
            = new HashSet<TournamentMatch>();
    }
}
