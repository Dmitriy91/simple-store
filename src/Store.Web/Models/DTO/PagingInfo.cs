using System;

#pragma warning disable 1591

namespace Store.Web.Models.DTO
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }

        public int PageSize { get; set; }

        public int TotalPages
        {
            get
            {
                if (PageSize == 0)
                    return 0;

                return (int)Math.Ceiling((decimal)TotalItems / PageSize);
            }
        }

        public int CurrentPage { get; set; }
    }
}

#pragma warning restore 1591