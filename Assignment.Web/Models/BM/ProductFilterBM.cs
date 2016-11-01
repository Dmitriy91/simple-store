using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment.Web.Infrastructure.ValidationAttributes;
using Assignment.Web.Models;

namespace Assignment.Web.Models
{
    public class ProductFilterBM : PaginationBM
    {
        [StringLength(128)]
        public string ProductName{ get; set; }

        [ProductSortByValidation]
        public string SortBy { get; set; }
    }
}
