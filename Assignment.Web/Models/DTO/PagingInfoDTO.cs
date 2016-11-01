using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment.Web.Models
{
    public class PagingInfoDTO
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