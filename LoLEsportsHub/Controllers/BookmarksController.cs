using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Bookmarks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoLEsportsHub.Controllers
{
    public class BookmarksController : BaseController
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(IBookmarkService bookmarkService)
        {
             this._bookmarkService = bookmarkService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                IEnumerable<BookmarkViewModel> userBookmarks = await this._bookmarkService
                    .GetUserBookmarksAsync(userId);
                return View(userBookmarks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string? matchId)
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                bool result = await this._bookmarkService
                    .AddMatchToUserBookmarksAsync(matchId, userId);
                if (result == false)
                {
                    return this.RedirectToAction(nameof(Index), "Match");
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string? matchId)
        {
            try
            {
                string? userId = this.GetUserId();
                if (userId == null)
                {
                    return this.Forbid();
                }

                bool result = await this._bookmarkService
                    .RemoveMatchFromBookmarksAsync(matchId, userId);
                if (result == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Index), "Match");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
