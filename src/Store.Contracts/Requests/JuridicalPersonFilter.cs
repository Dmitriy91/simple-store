using Store.Contracts.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

namespace Store.Contracts.Requests
{
    /// <summary>
    /// JuridicalPersonFilter
    /// </summary>
    public class JuridicalPersonFilter : CustomerFilter
    {
        /// <summary>
        /// LegalName
        /// </summary>
        [StringLength(64)]
        public string LegalName { get; set; }

        /// <summary>
        /// TIN
        /// </summary>
        [StringLength(64)]
        public string TIN { get; set; }

        /// <summary>
        /// SortBy
        /// </summary>
        [JuridicalPersonSortByValidation]
        public string SortBy { get; set; }
    }
}

#pragma warning restore 1591
