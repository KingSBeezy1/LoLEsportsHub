using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Match
{
    public class DeleteMatchViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        public string? Title { get; set; }

        public string? VodUrl { get; set; }
    }
}
