using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class CreateIssueModel
    {
        public IEnumerable<ClientModel> Clients { get; set; }
        public IEnumerable<SystemUserModel> Attendants { get; set; }
        public IssueModel Issue { get; set; }
    }
}
