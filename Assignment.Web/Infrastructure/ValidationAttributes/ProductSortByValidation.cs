using System.ComponentModel.DataAnnotations;

namespace Assignment.Web.Infrastructure.ValidationAttributes
{
    public class ProductSortByValidationAttribute : ValidationAttribute
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
                    case "productNameAsc":
                    case "productNameDesc":
                    case "unitPriceAsc":
                    case "unitPriceDesc":
                        return true;
                }
            }

            ErrorMessage = "SortBy parameter has an invalid value.";

            return false;
        }
    }
}
