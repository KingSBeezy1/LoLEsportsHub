using LoLEsportsHub.Web.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Core.Interfaces
{
    public interface IHomeService
    {
        Task<IEnumerable<HomeTournamentCardViewModel>> GetTrendingTournamentsAsync();
    }
}
