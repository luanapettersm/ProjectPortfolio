using ProjectPortfolio.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class FilterRequestModel
    {
        public string Search { get; set; } 
        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public Guid? IssueId { get; set; }
        public IssueStatusEnum IssueStatus { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();
        public string SortColumn { get; set; }
        public string SortDirection{ get; set; }
    }
}
