using ProjectPortfolio.Enumerators;

namespace ProjectPortfolio.Models
{
    public class SystemUserModel
    {
        public Guid Id { get; set; }
        public string DisplayName => $"{Name} {Surname}";
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public bool IsEnabled { get; set; }
        public BusinessRoleEnum BusinessRole { get; set; }
        public SystemRoleEnum SystemRole { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
