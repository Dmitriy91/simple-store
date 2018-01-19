using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Store.Contracts.Requests
{
    /// <summary>
    /// Product
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Id
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// ProductName
        /// </summary>
        [Required, StringLength(128)]
        public string ProductName { get; set; }

        /// <summary>
        /// UntiPrice
        /// </summary>
        [Required, Range(typeof(decimal), "0", "999999999999999")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// UnitesInStock
        /// </summary>
        [Range(0, int.MaxValue)]
        public int UnitsInStock { get; set; }
    }
}

#pragma warning restore 1591
