using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Store.Contracts.Requests
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
        public List<Details> OrderDetails { get; set; }

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

#pragma warning restore 1591
