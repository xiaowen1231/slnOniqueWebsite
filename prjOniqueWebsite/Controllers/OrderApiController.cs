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
        /// 根據orderId傳回orderstatus
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult OrderStatusNow(int orderId)
        {
            OrderStatusDto dto = dao.GetOrderStatus(orderId);
            return Json(dto);
        }


        public IActionResult GetOrderStatusOptions()
        {
            List<OrderStatusDto> dto =dao.GetAllOrderStatus();
            return Json(dto);


        }
        //public IActionResult UpdateOrderStatus
        
    }
}
