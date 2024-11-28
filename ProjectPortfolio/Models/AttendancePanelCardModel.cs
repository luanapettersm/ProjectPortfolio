using ProjectPortfolio.Enumerators;

namespace ProjectPortfolio.Models
{
    public class AttendancePanelCardModel
    {
        public IssueStatusEnum State { get; set; }
        public IEnumerable<IssueModel> List { get; set; }
    }
}
