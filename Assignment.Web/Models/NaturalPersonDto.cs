using System;

namespace Assignment.Web.Models
{
    public class NaturalPersonDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string SSN { get; set; }

        public string Birthdate { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }
    }
}
