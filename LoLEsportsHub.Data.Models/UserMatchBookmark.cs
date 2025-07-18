using Microsoft.AspNetCore.Identity;

namespace LoLEsportsHub.Data.Models
{
    public class UserMatchBookmark
    {
        public string ApplicationUserId { get; set; } = null!;
        public virtual IdentityUser ApplicationUser { get; set; } = null!;

        public Guid MatchId { get; set; }
        public virtual Match Match { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}