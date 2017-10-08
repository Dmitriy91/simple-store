using System.Collections.Generic;

namespace Store.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }

        public virtual JuridicalPerson JuridicalPerson { get; set; }

        public virtual NaturalPerson NaturalPerson { get; set; }

        public virtual IList<Order> Orders { get; set; }
    }
}
