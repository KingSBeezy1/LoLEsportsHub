using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.GCommon.ApplicationConstants;

namespace LoLEsportsHub.Web.ViewModels.Match
{
    public class MatchFormInputModel
    {
        public string Id { get; set; }
            = string.Empty;

        public string Title { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string ScheduledDate { get; set; } = null!;

        public string? VodUrl { get; set; }
            = $"/images/{NoImageUrl}";
    }
}
