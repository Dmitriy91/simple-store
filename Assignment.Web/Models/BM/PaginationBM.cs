using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class PaginationBM
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = int.MaxValue;
    }
}
