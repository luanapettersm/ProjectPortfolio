using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class AttendancePanelController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Card")]
        public IActionResult Card()
        {
            return PartialView("~/Views/AttendancePanel/Card.cshtml");
        }
    }
}