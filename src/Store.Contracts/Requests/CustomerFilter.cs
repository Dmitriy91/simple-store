using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Store.Contracts.Requests
{
    /// <summary>
    /// CustomerFilter
    /// </summary>
    public class CustomerFilter : Pagination
    {
        /// <summary>
        /// Country
        /// </summary>
        [StringLength(64)]
        public string Country { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        [StringLength(64)]
        public string Region { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [StringLength(64)]
        public string City { get; set; }

        /// <summary>
        /// StreetAddress
        /// </summary>
        [StringLength(64)]
        public string StreetAddress { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        [StringLength(64)]
        public string PostalCode { get; set; }
    }
}

#pragma warning restore 1591
