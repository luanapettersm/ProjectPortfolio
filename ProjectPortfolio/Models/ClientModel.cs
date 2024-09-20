using System.Text.RegularExpressions;

namespace ProjectPortfolio.Models
{
    public class ClientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long? CNPJ { get; set; }
        public long? CPF { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public bool MailFormat => ValidateMailAddress(Mail);
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }
        public bool IsEnabled { get; set; }

        public StateModel State { get; set; }
        public CountryModel Country { get; set; }

        public bool ValidateMailAddress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            return Regex.IsMatch(input, @"\A(?:[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}
