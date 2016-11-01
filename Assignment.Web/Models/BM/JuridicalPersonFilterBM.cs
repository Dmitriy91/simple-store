using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Models
{
    public class JuridicalPersonFilterBM : CustomerFilterBM
    {
        [StringLength(64)]
        public string LegalName { get; set; }

        [StringLength(64)]
        public string TIN { get; set; }

        [JuridicalPersonSortByValidation]
        public string SortBy { get; set; }
    }
}
