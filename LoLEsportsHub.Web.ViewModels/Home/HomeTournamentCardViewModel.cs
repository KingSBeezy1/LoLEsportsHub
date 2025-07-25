using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Home
{
    public class HomeTournamentCardViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Region { get; set; } = null!;
        public int MatchCount { get; set; }
    }
}
