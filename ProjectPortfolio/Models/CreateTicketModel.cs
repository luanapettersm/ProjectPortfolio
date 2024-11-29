using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class CreateTicketModel
    {
        public IEnumerable<ClientModel> Clients { get; set; }
        public IEnumerable<SystemUserModel> Attendants { get; set; }
        public IEnumerable<ClientProjectModel> Projects { get; set; }
        public IssueModel Issue { get; set; }
    }
}
