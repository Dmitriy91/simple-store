using System;

namespace Assignment.Entities
{
    public class NaturalPerson
    {
        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string SSN { get; set; }

        public DateTime? Birthdate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
