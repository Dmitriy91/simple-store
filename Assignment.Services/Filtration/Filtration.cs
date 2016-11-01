using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assignment.Services
{
    public class Filtration : IFiltration
    {
        private string _sortBy;

        public IDictionary<string, string> Filters { get; set; }

        public string SortBy
        {
            get
            {
                return _sortBy;
            }
            set
            {
                if (value != null)
                {
                    value = (value.First().ToString().ToUpper() + value.Substring(1));
                    string[] sortByParts = Regex.Split(value, @"(?<!^)(?=[A-Z])");
                    value = string.Join("", sortByParts.ToList().GetRange(0, sortByParts.Length - 1));

                    switch (value)
                    {
                        case "Country":
                        case "Region":
                        case "City":
                        case "StreetAddress":
                        case "PostalCode":
                            value = "Customer." + value;
                            break;
                    }

                    value += " " + sortByParts[sortByParts.Length - 1];

                    _sortBy = value;
                }
            }
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string this[string filterName]
        {
            get
            {
                if (Filters == null)
                    return null;

                string filterValue = null;

                Filters.TryGetValue(filterName, out filterValue);

                return filterValue;
            }
        }
    }
}
