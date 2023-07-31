using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Controllers
{
    public class ShoppingCartApiController : Controller
    {
        private readonly OniqueContext _context;
        private readonly ProductDao _dao;
        public ShoppingCartApiController(OniqueContext context)
        {
            _context = context;
            _dao = new ProductDao(_context);
        }

        public IActionResult UpdateOrderQty(int stockId, int originalQty, int updateQty, int shoppingCartId)
        {
            var vm = new UpdateShoppingQtyVM();
            vm.StockId = stockId;
            vm.OriginalQty = originalQty;
            vm.UpdateQty = updateQty;
            vm.ShoppingCartId = shoppingCartId;

            var cart = new ShoppingCartService(_context).UpdateOrderQty(vm);
            return Json(cart);
        }

        public IActionResult DeleteCart(int shoppingCartId)
        {
            HttpStatusVM vm = new HttpStatusVM();
            try
            {
                _dao.DeleteCartItem(shoppingCartId);
                vm.StatusCode = 200;
                vm.StatusMessage = "刪除成功";
            }
            catch (Exception ex)
            {
                vm.StatusCode = 500;
                vm.StatusMessage = "刪除失敗";
            }
            return Json(vm);
        }
    }
}
