using System.Collections.Generic;

#pragma warning disable 1591

namespace Store.Web.Models.DTO
{
    public class Order
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

#pragma warning restore 1591
