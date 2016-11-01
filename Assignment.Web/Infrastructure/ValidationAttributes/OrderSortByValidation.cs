using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Infrastructure.ValidationAttributes
{
    public class OrderSortByValidationAttribute : ValidationAttribute
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
                    case "idAsc":
                    case "idDesc":
                    case "orderDateAsc":
                    case "orderDateDesc":
                        return true;
                }
            }

            ErrorMessage = "SortBy parameter has an invalid value.";

            return false;
        }
    }
}
