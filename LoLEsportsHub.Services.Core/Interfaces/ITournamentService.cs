using LoLEsportsHub.Web.ViewModels.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Core.Interfaces
{
    public interface ITournamentService
    {
        Task<IEnumerable<UsersTournamentIndexViewModel>> GetAllTournamentsUserViewAsync();

        Task<TournamentBracketViewModel?> GetTournamentBracketAsync(string? tournamentId);

        Task<TournamentDetailsViewModel?> GetTournamentDetailsAsync(string? tournamentIdId);
    }
}
