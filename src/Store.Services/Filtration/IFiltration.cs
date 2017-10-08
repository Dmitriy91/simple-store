using System.Collections.Generic;

namespace Store.Services
{
    public interface IFiltration : IPagination
    {
        IDictionary<string, string> Filters { get; set; }

        string this[string filterName] { get; }

        string SortBy { get; set; }
    }
}
