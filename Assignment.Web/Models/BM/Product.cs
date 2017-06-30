using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models.BM
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
