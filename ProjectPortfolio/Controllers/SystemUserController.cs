using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class SystemUserController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public IActionResult Filter()
        {
            return PartialView("~/Views/SystemUser/List.cshtml");
        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id = new Guid())
        {
            return PartialView("~/Views/SystemUser/Edit.cshtml");
        }
    }
}