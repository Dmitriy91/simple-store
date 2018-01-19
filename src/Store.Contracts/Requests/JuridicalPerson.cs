using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Store.Contracts.Requests
{
    /// <summary>
    /// JuridicalPerson
    /// </summary>
    public class JuridicalPerson
    {
        /// <summary>
        /// Id
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        /// <summary>
        /// LegalName
        /// </summary>
        [Required, StringLength(64)]
        public string LegalName { get; set; }

        /// <summary>
        /// TIN
        /// </summary>
        [Required, StringLength(16)]
        public string TIN { get; set; } //INN

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

#pragma warning restore 1591
