using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Tournament;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.GCommon.ApplicationConstants;

namespace LoLEsportsHub.Services.Core
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository)
        {
            this._tournamentRepository = tournamentRepository;
        }
        public async Task<IEnumerable<UsersTournamentIndexViewModel>> GetAllTournamentsUserViewAsync()
        {
            IEnumerable<UsersTournamentIndexViewModel> allTournamentsUsersView = await this._tournamentRepository
                .GetAllAttached()
                .Select(t => new UsersTournamentIndexViewModel()
                {
                    Id = t.Id.ToString(),
                    Name = t.Name,
                    Region = t.Region,
                })
                .ToArrayAsync();

            return allTournamentsUsersView;
        }

        public async Task<TournamentBracketViewModel?> GetTournamentBracketAsync(string? tournamentId)
        {

            if (string.IsNullOrWhiteSpace(tournamentId))
                return null;

            var tournament = await this._tournamentRepository
                .GetAllAttached()
                .Include(t => t.Organizer)
                .Include(t => t.Matches)
                    .ThenInclude(tm => tm.Match)
                .FirstOrDefaultAsync(t => t.Id.ToString().ToLower() == tournamentId.ToLower());

            if (tournament == null)
                return null;

            return new TournamentBracketViewModel
            {
                TournamentName = tournament.Name,
                OrganizerName = tournament.Organizer?.User.UserName ?? "Unknown",
                Matches = tournament.Matches
                    .Select(tm => tm.Match)
                    .DistinctBy(m => m.Id)
                    .Select(m => new TournamentBracketMatchViewModel
                    {
                        Id = m.Id.ToString(),
                        Title = m.Title,
                        Region = m.Region,
                        MatchDate = m.MatchDate,
                        AvailableSlots = tournament.Matches.First(tm => tm.MatchId == m.Id).AvailableSlots,
                        VODUrl = m.VODUrl ?? $"/images/{NoImageUrl}"
                    })
                    .ToList()
            };
        }

        public async Task<TournamentDetailsViewModel?> GetTournamentDetailsAsync(string? tournamentId)
        {
            TournamentDetailsViewModel? tournamentDetails = null!;
            if (!String.IsNullOrWhiteSpace(tournamentId))
            {
                Tournament? tournament = await this._tournamentRepository
                    .GetAllAttached()
                    .Include(c => c.Matches)
                    .ThenInclude(cm => cm.Match)
                    .SingleOrDefaultAsync(c => c.Id.ToString().ToLower() == tournamentId.ToLower());
                if (tournament != null)
                {

                    tournamentDetails = new TournamentDetailsViewModel()
                    {
                        Name = tournament.Name,
                        Region = tournament.Region,
                        Matches = tournament.Matches
                            .Select(cm => cm.Match)
                            .DistinctBy(m => m.Id)
                            .Select(m => new TournamentDetailsMatchViewModel()
                            {
                                Title = m.Title,
                                ScheduleDate = m.MatchDate.ToString(AppDateFormat)
                            })
                            .ToArray(),
                    };
                }
            }
            return tournamentDetails;
        }
    }
}
