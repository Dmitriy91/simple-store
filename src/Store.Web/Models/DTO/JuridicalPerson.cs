#pragma warning disable 1591

namespace Store.Web.Models.DTO
{
    public class JuridicalPerson
    {
        public int Id { get; set; }

        public string LegalName { get; set; }

        public string TIN { get; set; } //INN

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }
    }
}

#pragma warning restore 1591
