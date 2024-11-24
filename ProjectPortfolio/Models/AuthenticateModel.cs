using ProjectPortfolio.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class AuthenticateModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public SystemRoleEnum Roles { get; set; }
    }
}
