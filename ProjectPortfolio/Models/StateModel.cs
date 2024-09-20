namespace ProjectPortfolio.Models
{
    public class StateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UF {  get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
