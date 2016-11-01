namespace Assignment.Web.Models
{
    public class JuridicalPersonDTO
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
