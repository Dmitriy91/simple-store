using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store.Web.Models.BM
{
    /// <summary>
    /// Order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Id
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        [Required, Range(0, int.MaxValue)]
        public int CustomerId { get; set; }

        /// <summary>
        /// OrderDetails
        /// </summary>
        [Required]
        public IEnumerable<Details> OrderDetails { get; set; }

        /// <summary>
        /// Details
        /// </summary>
        public class Details
        {
            /// <summary>
            /// ProductId
            /// </summary>
            public int ProductId { get; set; }

            /// <summary>
            /// Quantity
            /// </summary>
            public int Quantity { get; set; }
        }
    }
}
