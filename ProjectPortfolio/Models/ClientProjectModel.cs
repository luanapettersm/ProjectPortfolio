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

        public List<string> CreateValidator()
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(Title) && Title.Length < 3 || Title.Length > 50)
                messages.Add("O título deve ter entre 3 e 50 caracteres.");
            if (string.IsNullOrEmpty(Address))
                messages.Add("Necessário informar o endereço da obra.");
            if (Number.ToString() == null)
                messages.Add("Necessário informar o número.");
            if (string.IsNullOrEmpty(City))
                messages.Add("Necessário informar a cidade em que acontecerá a obra.");
            if (string.IsNullOrEmpty(ZipCode))
                messages.Add("Necessário informar o CEP.");

            return messages;
        }
    }
}
