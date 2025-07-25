using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Bookmarks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoLEsportsHub.GCommon.ApplicationConstants;

namespace LoLEsportsHub.Services.Core
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;

        public BookmarkService(IBookmarkRepository bookmarkRepository)
        {
            this._bookmarkRepository = bookmarkRepository;
        }

        public async Task<bool> AddMatchToUserBookmarksAsync(string? matchId, string? userId)
        {
            bool result = false;
            if (matchId != null && userId != null)
            {
                bool isMatchIdValid = Guid.TryParse(matchId, out Guid matchGuid);
                if (isMatchIdValid)
                {
                    ApplicationUserMatch? userMatchEntry = await this._bookmarkRepository
                        .GetAllAttached()
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId &&
                                                    aum.MatchId.ToString() == matchGuid.ToString());

                    if (userMatchEntry != null)
                    {
                        userMatchEntry.IsDeleted = false;

                        result =
                            await this._bookmarkRepository.UpdateAsync(userMatchEntry);
                    }
                    else
                    {
                        userMatchEntry = new ApplicationUserMatch()
                        {
                            ApplicationUserId = userId,
                            MatchId = matchGuid,
                        };

                        await this._bookmarkRepository.AddAsync(userMatchEntry);
                        result = true;

                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<BookmarkViewModel>> GetUserBookmarksAsync(string userId)
        {
            IEnumerable<BookmarkViewModel> userBookmarks = await this._bookmarkRepository
                .GetAllAttached()
                .Include(aum => aum.Match)
                .AsNoTracking()
                .Where(aum => aum.ApplicationUserId.ToLower() == userId.ToLower())
                .Select(aum => new BookmarkViewModel()
                {
                    MatchId = aum.MatchId.ToString(),
                    Title = aum.Match.Title,
                    Region = aum.Match.Region,
                    ScheduleDate = aum.Match.MatchDate.ToString(AppDateFormat),
                    VodUrl = aum.Match.VODUrl
                })
                .ToArrayAsync();

            return userBookmarks;
        }

        public async Task<bool> IsMatchAddedToBookmarks(string? matchId, string? userId)
        {
            bool result = false;  
            if (matchId != null && userId != null)
            {
                bool isMatchIdValid = Guid.TryParse(matchId, out Guid matchGuid);

                if (isMatchIdValid)
                {
                    ApplicationUserMatch? userMatchEntry = await this._bookmarkRepository
                        .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId &&
                                                     aum.MatchId.ToString() == matchGuid.ToString());

                    if (userMatchEntry != null)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        public async Task<bool> RemoveMatchFromBookmarksAsync(string? matchId, string? userId)
        {
            bool result = false;
            if (matchId != null && userId != null )
            {
                bool isMatchValid = Guid.TryParse(matchId, out Guid matchGuid);

                if (isMatchValid)
                {
                    ApplicationUserMatch? userMatchEntry = await this._bookmarkRepository
                        .SingleOrDefaultAsync(aum => aum.ApplicationUserId.ToLower() == userId &&
                                                     aum.MatchId.ToString() == matchGuid.ToString());

                    if (userMatchEntry != null)
                    {
                        result =
                            await this._bookmarkRepository.DeleteAsync(userMatchEntry);
                    }
                }
            }
            return result;
        }
    }
}
