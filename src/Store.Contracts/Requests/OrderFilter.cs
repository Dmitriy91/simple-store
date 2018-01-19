using Store.Contracts.ValidationAttributes;

#pragma warning disable 1591

namespace Store.Contracts.Requests
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

#pragma warning restore 1591
