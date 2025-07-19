using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Web.ViewModels.Tournament
{
    public class TournamentBracketMatchViewModel
    {
        public Guid Id { get; set; }             // TournamentMatch Id or Match Id
        public string Title { get; set; } = null!;
        public string Region { get; set; } = null!;
        public DateTime MatchDate { get; set; }
        public int AvailableSlots { get; set; }
        public string? VODUrl { get; set; }

    }
}
