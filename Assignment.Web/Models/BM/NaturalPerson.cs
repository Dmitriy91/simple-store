using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models.BM
{
    /// <summary>
    /// NaturalPerson
    /// </summary>
    public class NaturalPerson
    {
        /// <summary>
        /// Id
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        [Required, StringLength(32)]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        [Required, StringLength(32)]
        public string LastName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        [Required, StringLength(32)]
        public string MiddleName { get; set; }

        /// <summary>
        /// SSN
        /// </summary>
        [StringLength(32)]
        public string SSN { get; set; }

        /// <summary>
        /// Birthdate
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        [Required, StringLength(64)]
        public string Country { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        [Required, StringLength(64)]
        public string Region { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [Required, StringLength(64)]
        public string City { get; set; }

        /// <summary>
        /// StreetAddress
        /// </summary>
        [Required, StringLength(64)]
        public string StreetAddress { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        [StringLength(64)]
        public string PostalCode { get; set; }
    }
}
