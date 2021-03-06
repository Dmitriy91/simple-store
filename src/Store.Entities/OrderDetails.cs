﻿namespace Store.Entities
{
    public class OrderDetails
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        
        public virtual Order Order { get; set; }
    }
}
