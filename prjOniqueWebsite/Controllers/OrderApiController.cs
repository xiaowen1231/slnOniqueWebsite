using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class OrderApiController : Controller
    {
        private readonly OniqueContext _context;
        public OrderApiController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult showOrderList()
        {

            var query = from o in _context.Orders
                        join os in _context.OrderStatus
                        on o.OrderStatusId equals os.StatusId
                        join m in _context.Members
                        on o.MemberId equals m.MemberId
                        join pm in _context.PaymentMethods
                        on o.PaymentMethodId equals pm.PaymentMethodId
                        select new OrderListDto
                        {
                            StatusName = os.StatusName,
                            OrderId = o.OrderId,
                            Name = m.Name,
                            ShippingDate = (DateTime)o.ShippingDate,
                            PaymentMethodName = pm.PaymentMethodName,
                            PhotoPath = m.PhotoPath
                        };
//      select os.StatusName,OrderId,m.Name,ShippingDate,pm.PaymentMethodName,PhotoPath
//            from Orders o
//            join OrderStatus os
//on o.OrderStatusId = os.StatusId
//join Members m
//on o.MemberId = m.MemberId
//join PaymentMethods pm
//on o.PaymentMethodId = pm.PaymentMethodId
            List<OrderListDto> dto = query.ToList();
            return Json(dto);
        }
    }
}
