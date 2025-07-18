using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Models
{
    public class ApplicationUserMatch
    {
        public string ApplicationUserId { get; set; } = null!;
        public virtual IdentityUser ApplicationUser { get; set; } = null!;

        public Guid MatchId { get; set; }
        public virtual Match Match { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
