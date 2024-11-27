using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class IssueForwardingModel
    {
        public Guid IssueId { get; set; }
        public Guid? AttendantId { get; set; }
        public string Description { get; set; }
    }
}
