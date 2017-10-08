using System.Collections.Generic;

namespace Store.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }

        public virtual IList<OrderDetails> OrderDetails { get; set; }
    }
}
