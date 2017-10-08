using System.ComponentModel.DataAnnotations;

namespace Store.Web.Models.BM
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
