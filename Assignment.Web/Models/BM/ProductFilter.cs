using System.ComponentModel.DataAnnotations;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Models.BM
{
    /// <summary>
    /// ProductFilter
    /// </summary>
    public class ProductFilter : Pagination
    {
        /// <summary>
        /// ProductName
        /// </summary>
        [StringLength(128)]
        public string ProductName{ get; set; }

        /// <summary>
        /// SortBy
        /// </summary>
        [ProductSortByValidation]
        public string SortBy { get; set; }
    }
}
