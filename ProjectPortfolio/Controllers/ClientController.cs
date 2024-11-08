using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public IActionResult Filter()
        {
            return PartialView("~/Views/Client/List.cshtml");
        }
    }
}