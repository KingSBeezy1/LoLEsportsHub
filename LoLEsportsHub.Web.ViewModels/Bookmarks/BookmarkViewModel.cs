using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Bookmarks
{
    public class BookmarkViewModel
    {
        public string MatchId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string ScheduleDate { get; set; } = null!;

        public string? VodUrl { get; set; }
    }
}
