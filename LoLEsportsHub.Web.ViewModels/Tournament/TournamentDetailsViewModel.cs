using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Tournament
{
    public class TournamentDetailsViewModel
    {
        public string Name { get; set; } = null!;

        public string Region { get; set; } = null!;

        public IEnumerable<TournamentDetailsMatchViewModel> Matches { get; set; } = null!;
    }
}
