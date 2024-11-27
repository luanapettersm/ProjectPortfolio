using ProjectPortfolio.Enumerators;

namespace ProjectPortfolio.Models
{
    public class IssueModel
    {
        public Guid Id { get; set; }
        public long SequentialId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid? AttendantId { get; set; }
        public string AttendantName { get; set; }
        public string Description { get; set; }
        public PriorityEnum Priority { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateClosed { get; set; }
        public IssueStatusEnum Status { get; set; }
        public string Solution { get; set; }
        public ICollection<IssueNoteModel> Notes { get; set; }
    }
}
