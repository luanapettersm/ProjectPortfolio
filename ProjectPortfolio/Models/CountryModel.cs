namespace ProjectPortfolio.Models
{
    public class CountryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SpedCode { get; set; }
        public IEnumerable<StateModel> States { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
