using prjOniqueWebsite.Models.DTOs;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class OrderPagination
    {
        public List<OrderListDto> Orders { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public decimal OrderCount { get; set; }
        public int TotalPages=>(int)Math.Ceiling((double) OrderCount / PageSize);
    }
}
