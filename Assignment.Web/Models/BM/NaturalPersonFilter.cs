using System.ComponentModel.DataAnnotations;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Models.BM
{
    /// <summary>
    /// NaturalPersonFilter
    /// </summary>
    public class NaturalPersonFilter : CustomerFilter
    {
        /// <summary>
        /// FirstName
        /// </summary>
        [StringLength(32)]
        public string FirstName { get; set; }

        /// <summary>
        /// MiddleName
        /// </summary>
        [StringLength(32)]
        public string MiddleName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        [StringLength(32)]
        public string LastName { get; set; }

        /// <summary>
        /// SSN
        /// </summary>
        [StringLength(32)]
        public string SSN { get; set; }

        /// <summary>
        /// BirthDate
        /// </summary>
        [RegularExpression(@"^((19|20)\d\d)-(0?[1-9]|1[012])-(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Invalid date format. Only yyyy-mm-dd format is allowed.")]
        public string Birthdate { get; set; }

        /// <summary>
        /// SortBy
        /// </summary>
        [NaturalPersonSortByValidation]
        public string SortBy { get; set; }
    }
}
