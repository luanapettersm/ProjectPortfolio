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
        public string SystemRole { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
