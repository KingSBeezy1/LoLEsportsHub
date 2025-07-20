using Microsoft.AspNetCore.Mvc;

namespace LoLEsportsHub.Controllers
{
    public class OrganizerController : BaseController
    {
        public IActionResult Index()
        {
            return this.Ok("I am the manager!");
        }
    }
}
