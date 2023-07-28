﻿using Microsoft.AspNetCore.Mvc;
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
                data = query.Where(o => o.Name.Contains(keyWord)||o.StatusName.Contains(keyWord)||o.OrderId.ToString().Contains(keyWord)).ToList();

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


        public IActionResult GetOrderStatusOptions(string StatusName)
        {
            var query = _context.OrderStatus;

            switch (StatusName)
            {
                case "待出貨":
                     query.Where(s => s.StatusName == "已出貨" || s.StatusName == "已取消").ToList();
                    break;
                case "已出貨":
                     query.Where(s => s.StatusName == "已完成" || s.StatusName == "未取貨").ToList();
                    break;
                case "已完成":
                      //todo
                case "已取消":
                case "退款中":
                case "已退款":
                case "未取貨":
                default:
                    break;




            }

            return Json(/*data*/null);


        }
        //public IActionResult UpdateOrderStatus

    }
}
