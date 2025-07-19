using LoLEsportsHub.Data;
using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.GCommon.ApplicationConstants;

namespace LoLEsportsHub.Services.Core
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            this.matchRepository = matchRepository;
        }    

        public async Task AddMatchAsync(MatchFormInputModel inputModel)
        {
            
            Match newMatch = new Match()
            {
                Title = inputModel.Title,
                Region = inputModel.Region,
                VODUrl = inputModel.VodUrl,
                MatchDate = DateTime
                    .ParseExact(inputModel.ScheduledDate, AppDateFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None),
            };

            await this.matchRepository.AddAsync(newMatch);
        }

        public async Task<bool> DeleteMatchAsync(string? id)
        {
            Match? matchToDelete = await this.FindMatchByStringId(id);
            if (matchToDelete == null)
            {
                return false;
            }

            await this.matchRepository
                .HardDeleteAsync(matchToDelete);

            return true;
        }

        public async Task<bool> EditMatchAsync(MatchFormInputModel inputModel)
        {
            bool result = false;

            Match? editableMatch = await this.FindMatchByStringId(inputModel.Id);
            if (editableMatch == null)
            {
                return false;
            }

            DateTime matchScheduledDate = DateTime
                .ParseExact(inputModel.ScheduledDate, AppDateFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None);
            editableMatch.Title = inputModel.Title;
            editableMatch.Region = inputModel.Region;
            editableMatch.MatchDate = matchScheduledDate;
            editableMatch.VODUrl = inputModel.VodUrl;

            result = await this.matchRepository.UpdateAsync(editableMatch);

            return result;
        }

        public async Task<IEnumerable<AllMatchesIndexViewModel>> GetAllMatchesAsync()
        {
            IEnumerable<AllMatchesIndexViewModel> allMatches = await this.matchRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(m => new AllMatchesIndexViewModel()
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Region = m.Region,
                    ScheduledDate = m.MatchDate.ToString(AppDateFormat),
                    VODurl = m.VODUrl,
                })
                .ToListAsync();

            foreach (AllMatchesIndexViewModel match in allMatches)
            {
                if (String.IsNullOrEmpty(match.VODurl))
                {
                    match.VODurl = $"/images/{NoImageUrl}";
                }
            }
            return allMatches;

        }

        public async Task<MatchFormInputModel?> GetEditableMatchByIdAsync(string? id)
        {
            MatchFormInputModel? editableMatch = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid matchId);
            if (isIdValidGuid)
            {
                editableMatch = await this.matchRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(m => m.Id == matchId)
                    .Select(m => new MatchFormInputModel()
                    {
                        Id = m.Id.ToString(),
                        Title = m.Title,
                        Region = m.Region,
                        ScheduledDate = m.MatchDate.ToString(AppDateFormat),
                        VodUrl = m.VODUrl,
                    })
                    .SingleOrDefaultAsync();
            }
            return editableMatch;
        }

        public async Task<DeleteMatchViewModel> GetMatchDeleteDetailsByIdAsync(string? id)
        {
            DeleteMatchViewModel? deleteMatchViewModel = null;

            Match? matchToBeDeleted = await this.FindMatchByStringId(id);
            if (matchToBeDeleted != null)
            {
                deleteMatchViewModel = new DeleteMatchViewModel()
                {
                    Id = matchToBeDeleted.Id.ToString(),
                    Title = matchToBeDeleted.Title,
                    VodUrl = matchToBeDeleted.VODUrl ?? $"/images/{NoImageUrl}",
                };
            }

            return deleteMatchViewModel;
        }

        public async Task<MatchDetailsViewModel?> GetMatchDetailsByIdAsync(string? id)
        {
            MatchDetailsViewModel? matchDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid matchId);
            if (isIdValidGuid)
            {
                matchDetails = await this.matchRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(m => m.Id == matchId)
                    .Select(m => new MatchDetailsViewModel()
                    {
                        Id = m.Id.ToString(),
                        Title = m.Title,
                        Region = m.Region,
                        ScheduleDate = m.MatchDate.ToString(AppDateFormat),
                        VodUrl = m.VODUrl,
                    })
                    .SingleOrDefaultAsync();
            }
            return matchDetails;
        }

        public async Task<bool> SoftDeleteMatchAsync(string? id)
        {
            bool result = false;
            Match? matchToDelete = await this.FindMatchByStringId(id);
            if (matchToDelete == null)
            {
                return false;
            }

            result = await this.matchRepository.DeleteAsync(matchToDelete);

            return result;
        }

        private async Task<Match?> FindMatchByStringId(string? id)
        {
            Match? match = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid movieGuid);
                if (isGuidValid)
                {
                    match = await this.matchRepository
                        .GetByIdAsync(movieGuid);
                }
            }

            return match;
        }
    }
}
