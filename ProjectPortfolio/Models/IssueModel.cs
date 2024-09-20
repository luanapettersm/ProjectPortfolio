namespace ProjectPortfolio.Models
{
    public class IssueModel
    {
        public Guid Id { get; set; }
        public long SequentialId { get; set; }
        public Guid ClientId { get; set; }
        public Guid? AttendantId { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }

        public ICollection<IssueNoteModel> Notes { get; set; }
    }
}
