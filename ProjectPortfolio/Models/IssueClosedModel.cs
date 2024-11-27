using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class IssueClosedModel
    {
        public Guid IssueId { get; set; }
        public string Solution { get; set; }
        public Guid AttendantId { get; set; }
    }
}
