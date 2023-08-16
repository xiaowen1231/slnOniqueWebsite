using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
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
        private readonly UserInfoService _userInfoService;
        public ShoppingCartApiController(OniqueContext context, UserInfoService userInfoService)
        {
            _context = context;
            _dao = new ProductDao(_context);
            _userInfoService = userInfoService;
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
            ApiResult result = new ApiResult();
            try
            {
                _dao.DeleteCartItem(shoppingCartId);
                result.StatusCode = 200;
                result.StatusMessage = "刪除成功";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.StatusMessage = "刪除失敗";
            }
            return Json(result);
        }

        public IActionResult CartItems()
        {
            Members member = _userInfoService.GetMemberInfo();
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
            Members member = _userInfoService.GetMemberInfo();

            OrderConfirmationDto dto = new OrderConfirmationDto();
            dto.Name = member.Name;
            dto.Phone = member.Phone;
            dto.CityId = member.Citys;
            dto.AreaId = member.Areas;
            dto.Address = member.Address;

            return Json(dto);
        }

        public IActionResult CreateOrder(OrderConfirmationVM vm)
        {
            var dto = new OrderSettlementDto();
            var result = new ApiResult();
            try
            {
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

                string city = _context.Citys.Where(c => c.CityId == vm.City).Select(c => c.CityName).FirstOrDefault();
                string area = _context.Areas.Where(a => a.AreaId == vm.Area).Select(a => a.AreaName).FirstOrDefault();
                string shippingAddress = city + area + vm.Address;

                var member = _context.Members.FirstOrDefault(m => m.MemberId == vm.MemberId);

                var order = new Orders
                {
                    OrderId = orderId,
                    MemberId = vm.MemberId,
                    MethodId = vm.MethodId,
                    ShippingAddress = shippingAddress,
                    OrderStatusId = 1,
                    OrderDate = DateTime.Now,
                    PaymentMethodId = vm.PaymentMethodId,
                    Recipient = vm.Recipient,
                    Remark = vm.Remark,
                    RecipientPhone = vm.RecipientPhone,
                    TotalPrice = vm.TotalPrice,
                };
                var orderDetail = _dao.CartItems(member);

                foreach (var item in orderDetail)
                {
                    order.OrderDetails.Add(new OrderDetails
                    {
                        StockId = item.StockId,
                        OrderQuantity = item.ShoppingCart.OrderQuantity,
                        Price = item.DiscountMethod==null?item.Product.Price:Convert.ToDecimal(item.DiscountPrice)
                    });;
                }
                
                List<ShoppingCart> cartItems = new List<ShoppingCart>();
                cartItems = _context.ShoppingCart.Where(sc=>sc.MemberId == vm.MemberId).ToList();

                foreach (var item in cartItems)
                {
                    _context.ShoppingCart.Remove(item);
                }

                _context.Orders.Add(order);
                _context.SaveChanges();

                

                

                result.StatusCode = 200;
                result.StatusMessage = "恭喜您已完成訂購";

                dto.OrderId = orderId;
                dto.Result = result;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.StatusMessage = "訂購失敗" + ex.Message;
                dto.Result = result;

            }

            return Json(dto);
        }


    }
}
