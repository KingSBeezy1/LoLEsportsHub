using Microsoft.AspNetCore.Identity;

namespace LoLEsportsHub.Data.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Guid TournamentMatchId { get; set; }
        public virtual TournamentMatch TournamentMatch { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public virtual IdentityUser User { get; set; } = null!;
    }
}