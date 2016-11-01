using System;
using System.Collections.Generic;

namespace Assignment.Web.Models
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string OrderDate { get; set; }

        public IEnumerable<Details> OrderDetails { get; set; }

        public class Details
        {
            public int OrderId { get; set; }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public decimal UnitPrice { get; set; }

            public int Quantity { get; set; }
        }
    }
}
