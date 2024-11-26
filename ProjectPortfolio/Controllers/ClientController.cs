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

        [HttpGet("Editor/{id}")]
        public async Task<IActionResult> Editor(Guid id = new Guid())
        {
            var client = new ClientModel();

            if (id != Guid.Empty)
                client = await repository.GetAsync(id);

            return PartialView("");
        }
    }
}