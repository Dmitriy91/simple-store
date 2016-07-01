using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Models
{
    public class JuridicalPersonBindingModel
    {
        public int Id { get; set; }
        [Required, StringLength(64)]
        public string LegalName { get; set; }
        [Required, StringLength(16)]
        public string TIN { get; set; } //INN
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
