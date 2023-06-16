using OrderEat.Domain.Models;

namespace OrderEat.Domain.Models
{
    public class PagedResultDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchPhrase { get; set; }
        public string? SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}