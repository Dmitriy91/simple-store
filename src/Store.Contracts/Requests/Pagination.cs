using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Store.Contracts.Requests
{
    /// <summary>
    /// Pagination
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// PageNumber
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// PageSize
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = int.MaxValue;
    }
}

#pragma warning restore 1591
