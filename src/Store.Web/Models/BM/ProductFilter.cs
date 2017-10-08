using System.ComponentModel.DataAnnotations;
using Store.Web.Infrastructure.ValidationAttributes;

namespace Store.Web.Models.BM
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
