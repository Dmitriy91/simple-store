using System;
using System.Collections.Generic;

namespace Store.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual IList<OrderDetails> OrderDetails { get; set; }
    }
}
