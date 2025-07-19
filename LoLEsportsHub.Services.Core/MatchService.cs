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
    }
}
