using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class ClientController(IClientRepository repository) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public IActionResult Filter()
        {
            return PartialView("~/Views/Client/List.cshtml");
        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id = new Guid())
        {
            return PartialView("~/Views/Client/Edit.cshtml");
        }
    }
}