using Microsoft.AspNetCore.Mvc;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class LogoutController() : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //
            //

            return Redirect("/Home");
        }

    }
}
