using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Tournament;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoLEsportsHub.Controllers
{
    public class TournamentController : BaseController
    {
        private readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            this._tournamentService = tournamentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<UsersTournamentIndexViewModel> allTournamentsUserView = await this._tournamentService
                    .GetAllTournamentsUserViewAsync();

                return View(allTournamentsUserView);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Bracket(string? id)
        {
            try
            {
                TournamentBracketViewModel? tournamentBracket = await this._tournamentService
                    .GetTournamentBracketAsync(id);
                if (tournamentBracket == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(tournamentBracket);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                TournamentDetailsViewModel? tournamentBracket = await this._tournamentService
                    .GetTournamentDetailsAsync(id);

                if (tournamentBracket == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(tournamentBracket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
