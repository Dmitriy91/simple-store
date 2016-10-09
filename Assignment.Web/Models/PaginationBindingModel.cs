using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class PaginationBindingModel
    {
        public PaginationBindingModel()
        {
            PageSize = int.MaxValue;
            PageNumber = 1;
        }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
