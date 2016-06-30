using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class OrderBindingModel
    {
        public int Id { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public IList<Details> OrderDetails { get; set; }
        public class Details
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
