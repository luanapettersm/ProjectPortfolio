using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class CreateTicketModel
    {
        public List<ClientModel> Clients { get; set; }
        public List<SystemUserModel> Attendants { get; set; }
        public List<ClientProjectModel> Projects { get; set; }
        public IssueModel Issue { get; set; }
    }
}
