using System.Collections.Generic;

namespace Assignment.Web.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderDate { get; set; }
        public IList<Details> OrderDetails { get; set; }
        public class Details
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
        }
    }
}
