using LoLEsportsHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Repository.Interfaces
{
    public interface IBookmarkRepository 
        : IRepository<ApplicationUserMatch, object>, IAsyncRepository<ApplicationUserMatch, object>
    {
        ApplicationUserMatch? GetByCompositeKey(string userId, string matchId);

        Task<ApplicationUserMatch?> GetByCompositeKeyAsync(string userId, string matchId);

        bool Exists(string userId, string matchId);

        Task<bool> ExistsAsync(string userId, string matchId);
    }
}
