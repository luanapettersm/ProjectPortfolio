using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjectPortfolio.Models
{
    public class ClientModel
    {
        private string _cpf;
        private string _cnpj;
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        public string Name { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string Mail { get; set; }
        
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string City { get; set; }
        
        public string State { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public ICollection<ClientProjectModel> Projects { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
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
    }
}
