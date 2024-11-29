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
        public string Password { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public BusinessRoleEnum BusinessRole { get; set; }
        public SystemRoleEnum SystemRole { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }

        public List<string> Validator()
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(Name) || Name.Length < 3 || Name.Length > 35)
                messages.Add("Nome deve ter entre 3 e 35 caracteres.");
            if (string.IsNullOrEmpty(Surname) || Surname.Length < 3 || Surname.Length > 100)
                messages.Add("O sobrenome deve ter entre 3 e 100 caracteres.");
            if (string.IsNullOrEmpty(UserName) || UserName.Length < 3 || UserName.Length > 50)
                messages.Add("O login deve ter entre 3 e 50 caracteres.");
            if (Password == null)
                messages.Add("A senha é obrigatória.");
            if (BusinessRole.GetType() == null)
                messages.Add("O cargo é obrigatório.");

            return messages;
        }
    }
}