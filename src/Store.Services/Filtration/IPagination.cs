﻿namespace Store.Services
{
    public interface IPagination
    {
        int PageNumber { get; set; }

        int PageSize { get; set; }
    }
}
