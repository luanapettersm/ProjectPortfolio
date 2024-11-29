using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class ClientController(IClientRepository repository, 
        IClientProjectRepository projectRepository,
        IClientService service,
        IClientProjectService projectService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> Filter()
        {
            var clients = await repository.GetAllClients();

            var result = new
            {
                draw = 1,
                recordsTotal = clients.Count(),
                recordsFiltered = clients.Count(),
                data = clients
            };

            return Ok(result);

        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var client = new ClientModel();

            client = id.HasValue ? await repository.GetAsync((Guid)id) : null;

            return PartialView("~/Views/Client/Edit.cshtml", client);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save(ClientModel client)
        {
            try
            {
                var result = client.Id == Guid.Empty ? await service.CreateAsync(client) : await service.UpdateAsync(client);

                if (result != null)
                {
                    return Ok(new
                    {
                        Data = result, 
                        Message = client.Id == Guid.Empty
                            ? "Cliente salvo com sucesso."
                            : "Cliente atualizado com sucesso.",
                        Status = true
                    });
                }

                return BadRequest(new
                {
                    Data = null as ClientModel,
                    Message = "Falha ao salvar o cliente.",
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

        [HttpGet("Project/{clientId}")]
        public IActionResult Project(Guid clientId)
        {
            var model = new ProjectClientListModel
            {
                Id = clientId,
            };

            return PartialView("~/Views/Client/Project.cshtml", model);
        }

        [HttpGet("ProjectEdit")]
        [HttpGet("ProjectEdit/{id}")]
        public async Task<IActionResult> ProjectEdit(Guid? id)
        {
            //var project = new ClientProjectModel();

            var project = id.HasValue ? await projectRepository.GetAsync((Guid)id) : null;

            return PartialView("~/Views/Client/ProjectEdit.cshtml", project);
        }

        [HttpGet("ProjectList/{clientId}")]
        public async Task<IActionResult> ProjectList(Guid clientId)
        { 
            //var model = new List<ClientProjectModel>();

            var model = await projectRepository.GetAllClientProjects(clientId);

            return PartialView("~/Views/Client/ProjectList.cshtml", model);
        }

        [HttpPost("ProjectSave")]
        public async Task<IActionResult> ProjectSave(ClientProjectModel project)
        {
            try
            {
                var result = project.Id == Guid.Empty ? await projectService.CreateAsync(project) : await projectService.UpdateAsync(project);

                if (result != null)
                {
                    return Ok(new
                    {
                        Data = result,
                        Message = project.Id == Guid.Empty
                            ? "Projeto salvo com sucesso."
                            : "Projeto atualizado com sucesso.",
                        Status = true
                    });
                }

                return BadRequest(new
                {
                    Data = null as ClientProjectModel,
                    Message = "Falha ao salvar o projeto.",
                    Status = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Data = null as ClientProjectModel,
                    Message = $"Erro interno: {ex.Message}",
                    Status = false
                });
            }
        }
    }
}