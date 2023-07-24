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
                        where od.OrderId== orderId
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
        public IActionResult orderShippingDetail(int orderId)
        {
           
            var query = from o in _context.Orders
                        join os in _context.OrderStatus
                        on o.OrderStatusId equals os.StatusId
                        join sm in _context.ShippingMethods
                        on o.MethodId equals sm.MethodId
                        join pm in _context.PaymentMethods
                        on o.PaymentMethodId equals pm.PaymentMethodId
                        join m in _context.Members
                        on o.MemberId equals m.MemberId
                        where o.OrderId== orderId
                        select new OrderShippingDetailDto
                        {
                            Name = m.Name,
                            PhotoPath= m.PhotoPath,
                            Phone = m.Phone,
                            OrderId = o.OrderId,
                            ShippingAddress = o.ShippingAddress,
                            StatusName = os.StatusName,
                            MethodName = sm.MethodName,
                            PaymentMethodName = pm.PaymentMethodName,
                            OrderDate =o.OrderDate ,
                            ShippingDate = o.ShippingDate,
                            CompletionDate = o.CompletionDate,
                        };
            

           OrderShippingDetailDto dto = query.FirstOrDefault();
            if (dto != null)
            {
                return Json(dto);
            }
            else
            {
                return Content("無訂單資料"); // 如果找不到指定OrderId的資料，回傳NotFound
            }


        }

        /// <summary>
        /// 根據orderId內的orderstatusNow，傳回可選擇的orderstatusName
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult getOrderStatus(int orderId)
        {
            var statusNow = _context.Orders.Where(c => c.OrderId == orderId).Select(c => c.OrderStatusId).FirstOrDefault();
            if (statusNow == 1)
            {
                //var statusChoice=_context.ord
            }
            //var status = _context.OrderStatus.Select(s=>s.StatusName).ToList();
            return Json(statusNow);
        }
    }
}
