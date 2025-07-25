using LoLEsportsHub.Web.ViewModels.Bookmarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Core.Interfaces
{
    public interface IBookmarkService
    {
        Task<IEnumerable<BookmarkViewModel>> GetUserBookmarksAsync(string userId);

        Task<bool> AddMatchToUserBookmarksAsync(string? matchId, string? userId);

        Task<bool> RemoveMatchFromBookmarksAsync(string? matchId, string? userId);

        Task<bool> IsMatchAddedToBookmarks(string? matchId, string? userId);
    }
}
