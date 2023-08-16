using Microsoft.AspNetCore.Authorization;
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
        private readonly UserInfoService _userInfoService;
        public ShoppingCartController(OniqueContext context, UserInfoService userInfoService)
        {
            _context = context;
            _dao = new ProductDao(context);
            _userInfoService = userInfoService;
        }
        [Authorize(Roles = "Member")]
        public IActionResult Index()
        {
            Members member = _userInfoService.GetMemberInfo();
            List<ShoppingCartDto> cart = _dao.CartItems(member);
            return View(cart);
        }

        public IActionResult OrderConfirmation()
        {
            Members member = _userInfoService.GetMemberInfo();
            return View(member);
        }


        public IActionResult OrderSettlement(string orderId)
        {
            var dto = new OrderSettlementDto();
            var orderDetails = _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();

            decimal total = 0;

            foreach (var item in orderDetails)
            {
                total += (item.Price * item.OrderQuantity);
            }

            var query = from o in _context.Orders
                        join s in _context.ShippingMethods
                        on o.MethodId equals s.MethodId
                        where o.OrderId == orderId
                        select s.MethodName;

            string methodName = query.FirstOrDefault();


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

        public IActionResult ToEcpay(string orderId)
        {
            var ecpay = new Ecpay();
            var dto = _dao.GetOrderInfo(orderId);
            var data = ecpay.SubmitToEcpay(dto);
            return View(data);
        }
    }
}
