using LoLEsportsHub.Web.ViewModels.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Core.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<AllMatchesIndexViewModel>> GetAllMatchesAsync();

        Task AddMatchAsync(MatchFormInputModel inputModel);

        Task<MatchDetailsViewModel?> GetMatchDetailsByIdAsync(string? id);

        Task<MatchFormInputModel?> GetEditableMatchByIdAsync(string? id);

        Task<bool> EditMatchAsync(MatchFormInputModel inputModel);

        Task<DeleteMatchViewModel> GetMatchDeleteDetailsByIdAsync(string? id);

        Task<bool> SoftDeleteMatchAsync(string? id);

        Task<bool> DeleteMatchAsync(string? id);
    }
}
