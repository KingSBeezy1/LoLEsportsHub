using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LoLEsportsHub.Web.ViewModels.ValidationMessages.Match;

namespace LoLEsportsHub.Controllers
{
    public class MatchController : BaseController
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            this._matchService = matchService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var matches = await _matchService.GetAllMatchesAsync();
            return View(matches);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(MatchFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this._matchService.AddMatchAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                MatchDetailsViewModel? matchDetails = await this._matchService
                    .GetMatchDetailsByIdAsync(id);
                if (matchDetails == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return this.View(matchDetails);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                MatchFormInputModel? editableMatch = await this._matchService
                    .GetEditableMatchByIdAsync(id);
                if (editableMatch == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editableMatch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MatchFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this._matchService.EditMatchAsync(inputModel);
                if (!editSuccess)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            try
            {
                DeleteMatchViewModel? matchToBeDeleted = await this._matchService
                    .GetMatchDeleteDetailsByIdAsync(id);

                if (matchToBeDeleted == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(matchToBeDeleted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMatchViewModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                bool deleteResult = await this._matchService
                    .SoftDeleteMatchAsync(inputModel.Id);

                if (deleteResult == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }
    }
}
