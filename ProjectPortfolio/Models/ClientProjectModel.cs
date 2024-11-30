using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    public class ClientProjectModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }


        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string Address { get; set; }


        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string ZipCode { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "O número deve ser um valor válido e maior que zero.")]
        public int Number { get; set; }


        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string City { get; set; }

        public bool IsEnabled { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 50 caracteres.")]
        public string Title { get; set; }
        
        [ForeignKey(nameof(ClientId))]
        public ClientModel Client { get; set; }
    }
}
