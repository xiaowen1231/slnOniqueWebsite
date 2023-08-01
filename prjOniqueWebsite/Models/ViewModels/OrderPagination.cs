using prjOniqueWebsite.Models.DTOs;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class OrderPagination
    {
        public List<OrderListDto> Orders { get; set; }
        public int PageIndex { get; set; }
        public decimal PageSize { get; set; }
        public decimal OrderCount { get; set; }
        public decimal TotalPages { get; set; }
    }
}
