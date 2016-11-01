using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Models
{
    public class NaturalPersonFilterBM : CustomerFilterBM
    {
        [StringLength(32)]
        public string FirstName { get; set; }

        [StringLength(32)]
        public string MiddleName { get; set; }

        [StringLength(32)]
        public string LastName { get; set; }

        [StringLength(32)]
        public string SSN { get; set; }

        [RegularExpression(@"^((19|20)\d\d)-(0?[1-9]|1[012])-(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Invalid date format. Only yyyy-mm-dd format is allowed.")]
        public string Birthdate { get; set; }

        [NaturalPersonSortByValidation]
        public string SortBy { get; set; }
    }
}
