using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class FilterResponseModel<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}