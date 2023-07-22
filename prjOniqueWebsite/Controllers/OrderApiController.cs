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
                            OrderId =o.OrderId,
                            Name = m.Name,
                            ShippingDate = (DateTime)o.ShippingDate,
                            PaymentMethodName = pm.PaymentMethodName,
                            PhotoPath = m.PhotoPath
                        };

            List<OrderListDto> dto = query.ToList();
            return Json(dto);
        }
        public IActionResult orderProductDetail(int orderId)
        {
            var query = from od in _context.OrderDetails
                        join psd in _context.ProductStockDetails
                        on od.StockId equals psd.StockId
                        join pc in _context.ProductColors
                        on psd.ColorId equals pc.ColorId
                        join ps in _context.ProductSizes
                        on psd.SizeId equals ps.SizeId
                        join p in _context.Products
                        on psd.ProductId equals p.ProductId
                        select new OrderProductsList
                        {
                            ProductName = p.ProductName,
                            SizeName = ps.SizeName,
                            ColorName = pc.ColorName,
                            OrderQuantity = od.OrderQuantity,
                            Price = od.Price,
                            PhotoPath = p.PhotoPath
                        };
            List<OrderProductsList> dto= query.ToList(); 
            return Json(dto);
        }
        


    }
}
