using ProjectPortfolio.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace ProjectPortfolio.Models
{
    public class SystemUserModel
    {
        public Guid Id { get; set; }
        
        public string DisplayName => $"{Name} {Surname}";
        
        [Required(ErrorMessage = "O campo Nome e obrigatorio.")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 35 caracteres.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O campo Sobrenome e obrigatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O sobrenome deve ter entre 3 e 100 caracteres.")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "O campo Login e obrigatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O login deve ter entre 3 e 50 caracteres.")]
        public string UserName { get; set; }
        
        public bool IsEnabled { get; set; }

        [Required(ErrorMessage = "O campo Senha e obrigatorio.")]
        public string Password { get; set; }
        
        public DateTimeOffset DateCreated { get; set; }

        [Required(ErrorMessage = "O campo Cargo e obrigatorio.")]
        public BusinessRoleEnum BusinessRole { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}