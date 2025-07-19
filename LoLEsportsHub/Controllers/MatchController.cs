using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Match;
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
    }
}
