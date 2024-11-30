using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class SystemUserController(ISystemUserRepository repository,
        ISystemUserService service) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> Filter()
        {
            var users = await repository.GetAllSystemUsers();

            var result = new
            {
                draw = 1,
                recordsTotal = users.Count(),
                recordsFiltered = users.Count(),
                data = users
            };

            return Ok(result);

        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var systemUser = new SystemUserModel();

            systemUser = id.HasValue ? await repository.GetAsync((Guid)id) : null;

            return PartialView("~/Views/SystemUser/Edit.cshtml", systemUser);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(SystemUserModel systemUser)
        {
            try
            {
                var result = systemUser.Id == Guid.Empty ? await service.CreateAsync(systemUser) : await service.UpdateAsync(systemUser);

                if (result != null)
                {
                    return Ok(new
                    {
                        Data = result,
                        Message = systemUser.Id == Guid.Empty
                            ? "usuario salvo com sucesso."
                            : "usuario atualizado com sucesso.",
                        Status = true
                    });
                }

                return BadRequest(new
                {
                    Data = null as ClientModel,
                    Message = "Falha ao salvar o usuario.",
                    Status = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Data = null as ClientModel,
                    Message = $"Erro interno: {ex.Message}",
                    Status = false
                });
            }
        }

        [HttpGet("{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await service.DeleteAsync(id);

            return Ok();
        }

    }
}