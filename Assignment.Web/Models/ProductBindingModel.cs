using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class ProductBindingModel
    {
        public int Id { get; set; }
        [Required, StringLength(128)]
        public string ProductName { get; set; }
        [Required, Range(typeof(decimal),"0", "999999999999999")]
        public decimal UnitPrice { get; set; }
        [Range(0, int.MaxValue)]
        public int? UnitsInStock { get; set; }
    }
}
