using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    public class ClientProjectModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }


        [Required(ErrorMessage = "O endereco e obrigatorio.")]
        public string Address { get; set; }


        [Required(ErrorMessage = "O CEP e obrigatorio.")]
        public string ZipCode { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "O numero deve ser um valor valido e maior que zero.")]
        public int Number { get; set; }


        [Required(ErrorMessage = "A cidade e obrigatoria.")]
        public string City { get; set; }

        public bool IsEnabled { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "O titulo e obrigatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O titulo deve ter entre 3 e 50 caracteres.")]
        public string Title { get; set; }
        
        [ForeignKey(nameof(ClientId))]
        public ClientModel Client { get; set; }
    }
}
