using ProjectPortfolio.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class AttendancePanelCardModel
    {
        public IssueStatusEnum State { get; set; }
        public IEnumerable<IssueModel> List { get; set; }
    }
}
