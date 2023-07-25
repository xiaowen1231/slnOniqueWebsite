using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Controllers
{
    public class OrderApiController : Controller
    {
        private readonly OniqueContext _context;
        OrderDao dao = null;
        public OrderApiController(OniqueContext context)
        {
            _context = context;
            dao =new OrderDao(_context);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult showOrderList()
        {
            List<OrderListDto> dto = dao.getOrderList();
            return Json(dto);
        }

           

        public IActionResult orderProductDetail(int orderId)
        {

            List<OrderProductsListDto> dto = dao.getProductDetail(orderId);
            return Json(dto);
        }
        public IActionResult orderShippingDetail(int orderId)
        {
           OrderShippingDetailDto dto =dao.getShippingDetail(orderId);
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
        /// 根據orderId傳回orderstatusNow
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult OrderStatusNow(int orderId)
        {
            OrderStatusDto dto = dao.GetOrderStatus(orderId);
            return Json(dto);
        }
            




            



        /// <summary>
        /// 依據option's value=>statusId傳回可選擇的statusName
        /// </summary>
        /// <returns></returns>
        public IActionResult OrderStatusOptions(int orderId)
        {
            var query = from o in _context.Orders
                        join os in _context.OrderStatus
                        on o.OrderId equals os.StatusId
                        join pm in _context.PaymentMethods
                        on o.PaymentMethodId equals pm.PaymentMethodId
                        where o.OrderId == orderId
                        select new getStatusNameVM
                        {
                            OrderId = o.OrderId,
                            StatusName = os.StatusName,
                            StatusId = os.StatusId,
                            PaymentMethodId = pm.PaymentMethodId,

                        };

            List<getStatusNameVM> vm = query.ToList();
            return Json(vm);
        }
    }
}
