using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Core
{
    public class HomeService : IHomeService
    {
        private readonly ITournamentRepository tournamentRepository;

        public HomeService(ITournamentRepository tournamentRepository)
        {
            this.tournamentRepository = tournamentRepository;
        }

        public async Task<IEnumerable<HomeTournamentCardViewModel>> GetTrendingTournamentsAsync()
        {
            return await tournamentRepository
                .GetAllAttached()
                .Where(t => !t.IsDeleted)
                .OrderByDescending(t => t.Matches.Count)
                .Take(6)
                .Select(t => new HomeTournamentCardViewModel
                {
                    Id = t.Id.ToString(),
                    Name = t.Name,
                    Region = t.Region,
                    MatchCount = t.Matches.Count
                })
                .ToListAsync();
        }
    }
}
