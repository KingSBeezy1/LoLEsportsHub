using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Match
{
    public class AllMatchesIndexViewModel
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string ScheduledDate { get; set; } = null!;
        public string? VODurl { get; set; }
        public bool IsAddedToUserBookmarks { get; set; }


    }
}
