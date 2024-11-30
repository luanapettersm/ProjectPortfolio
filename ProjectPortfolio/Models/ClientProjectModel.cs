using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    public class ClientProjectModel
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        
        [ForeignKey(nameof(ClientId))]
        public ClientModel Client { get; set; }
    }
}
