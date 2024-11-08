using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class SystemUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public IActionResult Filter()
        {
            return PartialView("~/Views/SystemUser/List.cshtml");
        }
    }
}