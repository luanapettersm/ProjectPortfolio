using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProjectPortfolio.Models
{
    public class ClientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public bool MailFormat => ValidateMailAddress(Mail);
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }
        public bool IsEnabled { get; set; }

        [ForeignKey(nameof(StateId))]
        public StateModel State { get; set; }

        public bool ValidateMailAddress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            return Regex.IsMatch(input, @"\A(?:[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public string CNPJ =>
            $"{CNPJUnformatted.Substring(0, 2)}." +
            $"{CNPJUnformatted.Substring(2, 3)}." +
            $"{CNPJUnformatted.Substring(5, 3)}/" +
            $"{CNPJUnformatted.Substring(8, 4)}-" +
            $"{CNPJUnformatted.Substring(12, 2)}";

        public string CNPJUnformatted => RemoveMask(CNPJNumber);
        public long CNPJIntNumber => Convert.ToInt64(CNPJUnformatted);
        public string CNPJNumber { get; set; }
        public string Root => CNPJUnformatted.Substring(0, 8);

        public string CPF =>
            $"{CPFUnformatted.Substring(0, 3)}." +
            $"{CPFUnformatted.Substring(3, 3)}." +
            $"{CPFUnformatted.Substring(6, 3)}-" +
            $"{CPFUnformatted.Substring(9, 2)}";

        public string CPFUnformatted => RemoveMask(CPFNumber);
        public long CPFIntNumber => Convert.ToInt64(CPFUnformatted);
        public string CPFNumber { get; set; }

        public static string RemoveMask(string number)
        {
            return new Regex(@"[^\d]").Replace(number, "");
        }
    }
}
