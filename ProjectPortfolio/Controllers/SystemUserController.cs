using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class SystemUserController(ISystemUserRepository repository) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public IActionResult Filter()
        {
            var model = new List<SystemUserModel>();

            return PartialView("~/Views/SystemUser/List.cshtml", model);
        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var model = new SystemUserModel();

            //model = id.HasValue ? await repository.GetAsync((Guid)id) : null;

            return PartialView("~/Views/SystemUser/Edit.cshtml", model);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(SystemUserModel systemUser)
        {

            //var result = systemUser.Id == Guid.Empty ? await repository.InsertAsync(systemUser)
            //    : await repository.UpdateAsync(systemUser);

            //return result.Success ? Ok(result) : BadRequest(result.Error.Details ?? result.Error.Message);

            return Ok();
        }

        [HttpGet("{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await repository.DeleteAsync(id);

            return Ok();
        }


    }
}