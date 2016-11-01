using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class CustomerFilterBM : PaginationBM
    {
        [StringLength(64)]
        public string Country { get; set; }

        [StringLength(64)]
        public string Region { get; set; }

        [StringLength(64)]
        public string City { get; set; }

        [StringLength(64)]
        public string StreetAddress { get; set; }

        [StringLength(64)]
        public string PostalCode { get; set; }
    }
}
