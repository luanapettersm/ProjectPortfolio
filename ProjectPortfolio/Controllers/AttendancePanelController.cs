using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> ListCardOpen([FromQuery] IssueStatusEnum status = IssueStatusEnum.Opened)
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Opened,
                List = await repository.ListIssues(status)
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardPending")]
        public async Task<IActionResult> ListCardPending([FromQuery] IssueStatusEnum status = IssueStatusEnum.Pending)
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Pending,
                List = await repository.ListIssues(status)
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardInProgress")]
        public async Task<IActionResult> ListCardInProgress([FromQuery] IssueStatusEnum status = IssueStatusEnum.InProgress)
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.InProgress,
                List = await repository.ListIssues(status)
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardClose")]
        public async Task<IActionResult> ListCardClose([FromQuery] IssueStatusEnum status = IssueStatusEnum.Closed)
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Closed,
                List = await repository.ListIssues(status)
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