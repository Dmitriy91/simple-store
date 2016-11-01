using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Infrastructure.ValidationAttributes
{
    public class NaturalPersonSortByValidation : ValidationAttribute
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
                    case "firstNameAsc":
                    case "firstNameDesc":
                    case "middleNameAsc":
                    case "middleNameDesc":
                    case "lastNameAsc":
                    case "lastNameDesc":
                    case "ssnAsc":
                    case "ssnDesc":
                    case "birthdateAsc":
                    case "birthdateDesc":
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
