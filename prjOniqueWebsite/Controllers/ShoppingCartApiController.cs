using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;
using System.Text.Json;

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

        public IActionResult CartItems()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            var cartList = _dao.CartItems(member);
            return Json(cartList);
        }

        public IActionResult DisplayShippingMethod()
        {
            List<ShippingMethods> shippingMethods = _dao.DisplayShippingMethod();
            return Json(shippingMethods);
        }

        public IActionResult DisplayPaymentMethods()
        {
            List<PaymentMethods> paymentMethods = _dao.DisplayPaymentMethods();
            return Json(paymentMethods);
        }

        public IActionResult GetMemberInfo()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);

            OrderConfirmationDto dto = new OrderConfirmationDto();
            dto.Name = member.Name;
            dto.Phone = member.Phone;
            dto.CityId = member.Citys;
            dto.AreaId = member.Areas;
            dto.Address = member.Address;

            return Json(dto);
        }
    }
}
