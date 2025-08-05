using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Tournament
{
    public class TournamentBracketViewModel
    {
        public string TournamentName { get; set; } = null!;
        public string OrganizerName { get; set; } = "Unknown"; 

        public List<TournamentBracketMatchViewModel> Matches { get; set; }
            = new List<TournamentBracketMatchViewModel>();
    }
}
