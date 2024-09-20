namespace ProjectPortfolio.Models
{
    public class SystemUserModel
    {
        public Guid Id { get; set; }
        public string DisplayName => $"{Name} {Surname}";
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public bool IsEnabled { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
