using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Controllers
{
    [Route("[controller]")]
    public class AttendancePanelController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("ListCardOpen")]
        public IActionResult ListCardOpen()
        {
            var model = new AttendancePanelCardModel{

                State = IssueStatusEnum.Opened,
                List = new List<IssueModel>() // listagem dos opens
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardPending")]
        public IActionResult ListCardPending()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Pending,
                List = new List<IssueModel>() // listagem dos Pending
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardInProgress")]
        public IActionResult ListCardInProgress()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.InProgress,
                List = new List<IssueModel>() // listagem dos InProgress
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }

        [HttpGet("ListCardClose")]
        public IActionResult ListCardClose()
        {
            var model = new AttendancePanelCardModel
            {

                State = IssueStatusEnum.Closed,
                List = new List<IssueModel>() // listagem dos close
            };

            return PartialView("~/Views/AttendancePanel/Card.cshtml", model);
        }
    }
}