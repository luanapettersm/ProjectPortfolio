using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPortfolio.Models
{
    [NotMapped]
    public class FilterResponseModel<TEntity>
    {
        public int Total { get; set; }
        public int FilteredRecords { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
    }
}