﻿#pragma warning disable 1591

namespace Store.Contracts
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
    }
}

#pragma warning restore 1591
