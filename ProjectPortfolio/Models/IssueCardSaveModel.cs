using ProjectPortfolio.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class IssueCardSaveModel
    {
        public Guid Id { get; set; }
        public Guid? AttendantId { get; set; }
        public IssueStatusEnum Status { get; set; }
        public bool IsMovedInAttendancePanel { get; set; }
    }
}
