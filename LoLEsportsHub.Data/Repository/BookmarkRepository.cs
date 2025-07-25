using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Data.Repository
{
    public class BookmarkRepository
        : BaseRepository<ApplicationUserMatch, object>, IBookmarkRepository
    {
        public BookmarkRepository(LoLEsportsHubDbContext dbContext)
            : base(dbContext)
        {
            
        }

        public bool Exists(string userId, string matchId)
        {
            return this
                .GetAllAttached()
                .Any(aum => aum.ApplicationUserId.ToLower() == userId.ToLower() &&
                            aum.MatchId.ToString().ToLower() == matchId.ToLower());
        }

        public Task<bool> ExistsAsync(string userId, string matchId)
        {
            return this
                .GetAllAttached()
                .AnyAsync(aum => aum.ApplicationUserId.ToLower() == userId.ToLower() &&
                                aum.MatchId.ToString().ToLower() == matchId.ToLower());
        }

        public ApplicationUserMatch? GetByCompositeKey(string userId, string matchId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefault(aum => aum.ApplicationUserId.ToLower() == userId.ToLower() &&
                                        aum.MatchId.ToString().ToLower() == matchId.ToLower());
        }

        public Task<ApplicationUserMatch?> GetByCompositeKeyAsync(string userId, string matchId)
        {
            return this
                .GetAllAttached()
                .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId.ToLower() &&
                                            aum.MatchId.ToString().ToLower() == matchId.ToLower());
        }
    }
}
