namespace Core.Models
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public string? SearchTerm { get; set; }
        public string? FilterBy { get; set; }
        public string? FilterValue { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            SortOrder = "asc";
        }
        public PaginationFilter(int pageNumber, int pageSize, string sortBy = null, string sortOrder = "asc", string searchTerm = null, string filterBy = null, string filterValue = null)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
            SortBy = sortBy;
            SortOrder = sortOrder;
            SearchTerm = searchTerm;
            FilterBy = filterBy;
            FilterValue = filterValue;
        }
    }
}