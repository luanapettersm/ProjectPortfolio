using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("Filter")]
        //public IActionResult Filter()
        //{
        //    return PartialView("~/Views/Client/List.cshtml");
        //}
    }
}