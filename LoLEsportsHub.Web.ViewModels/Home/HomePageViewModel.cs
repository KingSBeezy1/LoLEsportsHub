using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Home
{
    public class HomePageViewModel
    {
        public IEnumerable<HomeTournamentCardViewModel> TrendingTournaments { get; set; } = [];

    }
}
