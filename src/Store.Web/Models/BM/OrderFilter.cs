using Store.Web.Infrastructure.ValidationAttributes;

namespace Store.Web.Models.BM
{
    /// <summary>
    /// OrderFilter
    /// </summary>
    public class OrderFilter : Pagination
    {
        /// <summary>
        /// OrderDate
        /// </summary>
        public string OrderDate { get; set; }

        /// <summary>
        /// SortBy
        /// </summary>
        [OrderSortByValidation]
        public string SortBy { get; set; }
    }
}
