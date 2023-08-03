using prjOniqueWebsite.Models.Infra;

namespace prjOniqueWebsite.Models.Dtos
{
    public class OrderSettlementDto
    {
        public int OrderId { get; set; }
        public ApiResult Result { get; set; }
        public decimal Total { get; set; }
    }
}
