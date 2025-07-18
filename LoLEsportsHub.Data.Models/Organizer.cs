using Microsoft.AspNetCore.Identity;

namespace LoLEsportsHub.Data.Models
{
    public class Organizer
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }

        public string UserId { get; set; } = null!;
        public IdentityUser User { get; set; } = null!;

        public ICollection<Tournament> ManagedTournaments { get; set; }
            = new HashSet<Tournament>();
    }
}