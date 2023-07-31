using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.ViewModels
{
    public class UpdateShoppingQtyVM
    {
        public int StockId { get; set; }
        public int OriginalQty{ get; set; }
        public int UpdateQty { get; set; }
        public int ShoppingCartId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
