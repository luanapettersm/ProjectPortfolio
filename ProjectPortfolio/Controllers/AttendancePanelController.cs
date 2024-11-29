using Microsoft.AspNetCore.Authorization;
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
        IIssueNoteService noteService) : Controller
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
            //var client = new CreateTicketModel();

            // preencher campos da model CreateTicketModel

            var client = id.HasValue ? await repository.GetAsync((Guid)id) : null;

            return PartialView("~/Views/AttendancePanel/Edit.cshtml", client);
        }

        [HttpGet("GetProjectsByClient/{clientId}")]
        public async Task<IActionResult> GetProjectsByClient(Guid clientId)
        {
            var model = new List<ClientProjectModel>();

            return Ok(model);
        }

        [HttpGet("ListCardOpen")]
        public IActionResult ListCardOpen()
        {
            var model = new AttendancePanelCardModel{

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
        public async Task<IActionResult> Delete(Guid id, IssueStatusEnum status)
        {
            return Ok();
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save(IssueModel ticket)
        {
            try
            {
                var result = ticket.Id == Guid.Empty ? await service.CreateAsync(ticket) : null; //await service.UpdateAsync(ticket);

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

        [HttpGet("Note/{ticketId}")]
        public IActionResult Project(Guid ticketId)
        {
            var model = new NotePanelModel
            {
                Id = ticketId,
            };

            return PartialView("~/Views/AttendancePanel/Note.cshtml", model);
        }

        [HttpGet("NoteEdit")]
        [HttpGet("NoteEdit/{id}")]
        public async Task<IActionResult> NoteEdit(Guid? id)
        {
            var model = new IssueNoteModel();

            //var model = id.HasValue ? await noteService.GetAsync((Guid)id) : null;

            return PartialView("~/Views/AttendancePanel/NoteEdit.cshtml", model);
        }

        [HttpGet("NoteList/{ticketId}")]
        public async Task<IActionResult> NoteList(Guid ticketId)
        {
            var model = new List<IssueNoteModel>();

            //var model = await noteService.GetAllClientProjects(clientId);

            return PartialView("~/Views/AttendancePanel/NoteList.cshtml", model);
        }

        [HttpPost("NoteSave")]
        public async Task<IActionResult> NoteSave(IssueNoteModel note)
        {
            try
            {
                var result = note.Id == Guid.Empty ? await noteService.CreateAsync(note) : null; //await noteService.UpdateAsync(note);

                if (result != null)
                {
                    return Ok(new
                    {
                        Data = result,
                        Message = note.Id == Guid.Empty
                            ? "Nota salva com sucesso."
                            : "Nota atualizada com sucesso.",
                        Status = true
                    });
                }

                return BadRequest(new
                {
                    Data = null as IssueNoteModel,
                    Message = "Falha ao salvar a nota.",
                    Status = false
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Data = null as IssueNoteModel,
                    Message = $"Erro interno: {ex.Message}",
                    Status = false
                });
            }
        }

        [HttpGet("{id}/NoteDelete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //await noteService.DeleteAsync(id);

            return Ok();
        }
    }
}