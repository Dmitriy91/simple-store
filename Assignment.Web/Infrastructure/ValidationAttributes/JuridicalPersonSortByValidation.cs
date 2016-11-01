using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Infrastructure.ValidationAttributes
{
    public class JuridicalPersonSortByValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            if (value is string)
            {
                string sortBy = (string)value;

                switch (sortBy)
                {
                    case "legalNameAsc":
                    case "legalNameDesc":
                    case "tinAsc":
                    case "tinDesc":
                    case "countryAsc":
                    case "countryDesc":
                    case "regionAsc":
                    case "regionDesc":
                    case "cityAsc":
                    case "cityDesc":
                    case "streetAddressAsc":
                    case "streetAddressDesc":
                    case "postalCodeAsc":
                    case "postalCodeDesc":
                        return true;
                }
            }

            ErrorMessage = "SortBy parameter has an invalid value.";

            return false;
        }
    }
}
