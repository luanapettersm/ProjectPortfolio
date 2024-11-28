using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ProjectPortfolio.Models
{
    public class ClientModel
    {
        private string _cpf;
        private string _cnpj;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public bool MailFormat => ValidateMailAddress(Mail);
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsEnabled { get; set; }
        public ICollection<ClientProjectModel> Projects { get; set; }

        public bool ValidateMailAddress(string input)
        {
            if (string.IsNullOrEmpty(input))
                return true;

            return Regex.IsMatch(input, @"\A(?:[a-z0-9!#$%&'+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'+/=?^_`{|}~-]+)@(?:[a-z0-9](?:[a-z0-9-][a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public string CPF
        {
            get => _cpf;
            set
            {
                if (!string.IsNullOrEmpty(value) && !IsValidCPF(value))
                    throw new ArgumentException("CPF inválido.");
                _cpf = RemoveFormatting(value);
            }
        }

        public string CNPJ
        {
            get => _cnpj;
            set
            {
                if (!string.IsNullOrEmpty(value) && !IsValidCNPJ(value))
                    throw new ArgumentException("CNPJ inválido.");
                _cnpj = RemoveFormatting(value);
            }
        }

        public string cpfformatado => !string.IsNullOrEmpty(_cpf) ? FormatCPF(_cpf) : string.Empty;

        public string cnpjformatado => !string.IsNullOrEmpty(_cnpj) ? FormatCNPJ(_cnpj) : string.Empty;

        private static bool IsValidCPF(string cpf)
        {
            cpf = RemoveFormatting(cpf);
            return Regex.IsMatch(cpf, @"^\d{11}$");
        }

        private static bool IsValidCNPJ(string cnpj)
        {
            cnpj = RemoveFormatting(cnpj);
            return Regex.IsMatch(cnpj, @"^\d{14}$");
        }

        private static string FormatCPF(string cpf)
        {
            return Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
        }

        private static string FormatCNPJ(string cnpj)
        {
            return Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
        }

        private static string RemoveFormatting(string value)
        {
            return Regex.Replace(value ?? string.Empty, @"\D", "");
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(CPF) ? cpfformatado : cnpjformatado;
        }

        public List<string> Validator()
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(ZipCode))
                messages.Add("O CEP é obrigatório.");
            if (string.IsNullOrEmpty(Address))
                messages.Add("O endereço é obrigatório.");
            if (string.IsNullOrEmpty(PhoneNumber))
                messages.Add("O número é obrigatório.");
            if (string.IsNullOrEmpty(City))
                messages.Add("A cidadde é obrigatória.");
            if (string.IsNullOrEmpty(State))
                messages.Add("O estado é obrigatório.");

            return messages;
        }
    }
}
