using System;

#pragma warning disable 1591

namespace Store.Contracts.Responses
{
    public class NaturalPerson
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string SSN { get; set; }

        public DateTime? Birthdate { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }
    }
}

#pragma warning restore 1591
