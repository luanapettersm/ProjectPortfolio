using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class FilterRequestModel
    {
        public string Search { get; set; } 
        public int Page { get; set; } 
        public int PageSize { get; set; } 
    }
}
