using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591

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

#pragma warning restore 1591
