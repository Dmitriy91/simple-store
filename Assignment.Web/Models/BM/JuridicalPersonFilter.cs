﻿using System.ComponentModel.DataAnnotations;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Models.BM
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
