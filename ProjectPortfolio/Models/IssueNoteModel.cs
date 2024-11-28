using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    public class IssueNoteModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public Guid SystemUserId {  get; set; }
        public Guid IssueId { get; set; }

        [ForeignKey(nameof(IssueId))]
        public IssueModel Issue { get; set; }

        public List<string> CreateValidator()
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(Description) && Description.Length < 5 && Description.Length > 2000)
                messages.Add("A descrição deve ter entre 5 e 2000 caracteres");
            
            return messages;
        }
    }
}