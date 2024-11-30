using ProjectPortfolio.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace ProjectPortfolio.Models
{
    public class SystemUserModel
    {
        public Guid Id { get; set; }
        
        public string DisplayName => $"{Name} {Surname}";
        
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(35, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 35 caracteres.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O campo Sobrenome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O sobrenome deve ter entre 3 e 100 caracteres.")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "O campo Login é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O login deve ter entre 3 e 50 caracteres.")]
        public string UserName { get; set; }
        
        public bool IsEnabled { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Password { get; set; }
        
        public DateTimeOffset DateCreated { get; set; }

        [Required(ErrorMessage = "O campo Cargo é obrigatório.")]
        public BusinessRoleEnum BusinessRole { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}