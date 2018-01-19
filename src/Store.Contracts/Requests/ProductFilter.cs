using System.ComponentModel.DataAnnotations;
using Store.Contracts.ValidationAttributes;

#pragma warning disable 1591

namespace Store.Contracts.Requests
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

#pragma warning restore 1591
