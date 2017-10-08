namespace Store.Services
{
    public class Pagination : IPagination
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
