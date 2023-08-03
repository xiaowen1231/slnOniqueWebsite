using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Repositories;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly OniqueContext _context;
        public readonly ProductDao _dao;
        public ShoppingCartController(OniqueContext context)
        {
            _context = context;
            _dao = new ProductDao(context);
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult Index()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            List<ShoppingCartDto> cart = _dao.CartItems(member);
            return View(cart);
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult OrderConfirmation()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            return View(member);
        }

        [TypeFilter(typeof(MemberVerify))]
        public IActionResult OrderSettlement(int orderId)
        {
            var dto = new OrderSettlementDto();
            var orderDetails = _context.OrderDetails.Where(od=>od.OrderId == orderId).ToList();

            decimal total = 0;
            foreach (var item in orderDetails)
            {
                total += (item.Price * item.OrderQuantity);
            }

            string methodName = (from o in _context.Orders
                                 join sm in _context.ShippingMethods
                                 on o.MethodId equals sm.MethodId
                                 where o.OrderId == orderId
                                 select sm.MethodName).ToString();
            
            if (methodName == "宅配")
            {
                total += 100;
            }
            else if (methodName == "郵寄")
            {
                total += 120;
            }
            else
            {
                total += 60;
            }

            dto.OrderId = orderId;
            dto.Total = total;

            return View(dto);
        }
    }
}
