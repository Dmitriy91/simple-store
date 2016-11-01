using System.Collections.Generic;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Models
{
    public class OrderFilterBM : PaginationBM
    {
        public string OrderDate { get; set; }

        [OrderSortByValidation]
        public string SortBy { get; set; }
    }
}
