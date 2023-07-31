using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Services
{
    public class ShoppingCartService
    {
        private readonly ProductDao _dao;
        public ShoppingCartService(OniqueContext context)
        {
            _dao = new ProductDao(context);
        }
        public UpdateShoppingQtyVM UpdateOrderQty(UpdateShoppingQtyVM vm)
        {
            var psdInDb = _dao.GetProductStock(vm.StockId);

            if (vm.UpdateQty <= 0)
            {
                vm.StatusCode = 500;
                vm.Message = "訂購數量不可小於1件!";
                return vm;
            }
            else if (vm.UpdateQty > psdInDb.Quantity)
            {
                vm.StatusCode = 500;
                vm.Message = "訂購數量不可超過庫存數量!";
                return vm;
            }
            else
            {
               
                _dao.UpdateOrderQty(vm);
                vm.StatusCode = 200;
                vm.Message = "更新購物車數量成功!";
                return vm;
            }
        }

    }
}
