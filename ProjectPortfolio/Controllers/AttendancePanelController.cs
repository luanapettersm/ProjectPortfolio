using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class AttendancePanelController(IIssueService service,
        IIssueRepository repository,
        ISystemUserRepository systemUserRepository,
        IClientRepository clientRepository,
        IClientProjectRepository clientProjectRepository) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var clients = await clientRepository.GetListAsync();
            var model = new CreateTicketModel
            {
                Attendants = await systemUserRepository.GetListAsync(),
                Clients = clients,
                Issue = id.HasValue ? await repository.GetAsync((Guid)id) : null,
            };

            return PartialView("~/Views/AttendancePanel/Edit.cshtml", model);
        }

        [HttpGet("GetProjectsByClient/{clientId}")]
        public async Task<IActionResult> GetProjectsByClient(Guid clientId)
        {
            var model = await clientProjectRepository.GetAllClientProjects(clientId);

            return Ok(model);
        }

        [HttpGet("ListCardOpen")]
        public IActionResult ListCardOpen()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Opened,
                List = new List<IssueModel>()
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardPending")]
        public IActionResult ListCardPending()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Pending,
                List = new List<IssueModel>()
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardInProgress")]
        public IActionResult ListCardInProgress()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.InProgress,
                List = new List<IssueModel>()
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardClose")]
        public IActionResult ListCardClose()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Closed,
                List = new List<IssueModel>()
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ChangeStatusCard/{id}/{status}")]
        public async Task<IActionResult> ChangeStatusCard(Guid id, IssueStatusEnum status)
        {
            await repository.ChangeStatusCard(id, status);
            return Ok();
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save(IssueModel ticket)
        {
            try
            {
                var result = ticket.Id == Guid.Empty ? await service.CreateAsync(ticket) : await service.UpdateAsync(ticket);

                if (result != null)
                {
                    return Ok(new
                    {
                        Data = result,
                        Message = ticket.Id == Guid.Empty
                            ? "Chamado salvo com sucesso."
                            : "Chamado atualizado com sucesso.",
                        Status = true
                    });
                }

                return BadRequest(new
                {
                    Data = null as IssueModel,
                    Message = "Falha ao salvar o chamado.",
                    Status = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Data = null as IssueModel,
                    Message = $"Erro interno: {ex.Message}",
                    Status = false
                });
            }
        }
    }
}