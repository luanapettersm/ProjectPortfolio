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
    }
}