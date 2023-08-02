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
            dao = new OrderDao(_context);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult showOrderList()
        {
            List<OrderListDto> dto = dao.getOrderList().ToList();
            return Json(dto);
        }
        /// <summary>
        /// 分頁
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IActionResult orderPage(int page,int pageSize,string sort) 
        {
            List<OrderListDto> dto;
            switch (sort)
            {
                case "ascending":
                    dto = dao.getOrderList().OrderBy(order => order.OrderDate)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "descending":
                    dto = dao.getOrderList().OrderByDescending(order => order.OrderDate)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    dto = dao.getOrderList().OrderBy(order => order.OrderId)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }
                
            return Json(dto);
        }
        public IActionResult search(string? keyWord)
        {
            IEnumerable<OrderListDto> data = null;

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
                            OrderDate=o.OrderDate,
                            ShippingDate = (DateTime)o.ShippingDate,
                            PaymentMethodName = pm.PaymentMethodName,
                            PhotoPath = m.PhotoPath
                        };


            if (string.IsNullOrEmpty(keyWord))
            {
                data = query.ToList();
            }
            else
            {
                data = query.Where(o => o.Name.Contains(keyWord) || o.StatusName.Contains(keyWord) || o.OrderId.ToString().Contains(keyWord)).ToList();

            }
            return Json(data);
        }


        public IActionResult orderProductDetail(int orderId)
        {

            List<OrderProductsListDto> dto = dao.getProductDetail(orderId);
            return Json(dto);
        }
        public IActionResult orderShippingDetail(int orderId)
        {
            OrderShippingDetailDto dto = dao.getShippingDetail(orderId);
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
        /// 根據orderId傳回orderstatus
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult OrderStatusNow(int orderId)
        {
            OrderStatusDto dto = dao.GetOrderStatus(orderId);
            return Json(dto);
        }
        public IActionResult tags()
        {
            var tag = _context.Members.Select(c => c.Name).ToList();
            return Json(tag);
        }
        /// <summary>
        /// 依照orderId找出目前的statusName，再給予依照商業邏輯正確的statusName的選擇，傳回的是選項，不是order的資料
        /// 要把pay的方式列入考慮
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public IActionResult GetOrderStatusOptions(int OrderId)
        {

            var statusNow = dao.GetOrderStatus(OrderId).StatusName;
            var paymentMethodNow = dao.GetOrderStatus(OrderId).PaymentMethodName;
            var orderStatus = from o in _context.Orders
                        join os in _context.OrderStatus
                        on o.OrderStatusId equals os.StatusId
                        join pm in _context.PaymentMethods
                        on o.PaymentMethodId equals pm.PaymentMethodId
                        where o.OrderId == OrderId
                        select new OrderStatusDto
                        {
                            OrderId = o.OrderId,
                            StatusName = os.StatusName,
                            PaymentMethodName = pm.PaymentMethodName,
                            StatusId = os.StatusId
                        };
            var query = _context.OrderStatus;

            List<OrderStatus> data=null;


            if (statusNow == "待出貨")
            {
                data = query.Where(c => c.StatusName == "已出貨" || c.StatusName == "已取消").ToList();
            }
            if (statusNow == "已出貨")
            {
                data = query.Where(c => c.StatusName == "未取貨" || c.StatusName == "已完成").ToList();
            }


            if (statusNow == "已完成")
            {
                data = query.Where(c => c.StatusName == "退款中").ToList();
            }
            if (statusNow == "已取消" && paymentMethodNow != "貨到付款")
            {
                data = query.Where(c => c.StatusName == "退款中").ToList();
            }
            
            if(statusNow == "已取消" && paymentMethodNow == "貨到付款")
            {
                data = null;
            }
            if (statusNow == "退款中")
            {
                data = query.Where(c => c.StatusName == "已退款").ToList();
            }
            if (statusNow == "已退款")
            {
                data = null;
            }
            if (statusNow == "未取貨" && paymentMethodNow == "貨到付款")
            {
                data = query.Where(c => c.StatusName == "已取消").ToList();
            }
            if (statusNow == "未取貨" && paymentMethodNow != "貨到付款")
            {
                data = query.Where(c => c.StatusName == "退款中").ToList();
            }


            return Json(data);
        }
    }
}
