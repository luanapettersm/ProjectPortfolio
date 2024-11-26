using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    public class AttendancePanelController() : Controller
    {
        public async Task<IActionResult> Index()
        {

            return View("");
        }
    }
}