using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class NaturalPersonBM
    {
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required, StringLength(32)]
        public string FirstName { get; set; }

        [Required, StringLength(32)]
        public string LastName { get; set; }

        [Required, StringLength(32)]
        public string MiddleName { get; set; }

        [StringLength(32)]
        public string SSN { get; set; }

        public DateTime? Birthdate { get; set; }

        [Required, StringLength(64)]
        public string Country { get; set; }

        [Required, StringLength(64)]
        public string Region { get; set; }

        [Required, StringLength(64)]
        public string City { get; set; }

        [Required, StringLength(64)]
        public string StreetAddress { get; set; }

        [StringLength(64)]
        public string PostalCode { get; set; }
    }
}
