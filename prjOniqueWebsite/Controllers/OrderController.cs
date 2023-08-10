using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.ViewModels;
using prjOniqueWebsite.Models.Daos;
using System.Drawing.Printing;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using prjOniqueWebsite.Models.Infra;

namespace prjOniqueWebsite.Controllers
{
    public class OrderController : Controller
    {
        private readonly OniqueContext _context;
        private readonly UserInfoService _userInfoService;

        OrderDao dao = null;
        public OrderController(OniqueContext context, UserInfoService userInfoService)
        {
            _context = context;
            dao = new OrderDao(_context);
            _userInfoService = userInfoService;
        }

        [Authorize(Roles = "一般員工")]
        /// <summary>
        /// orderList的展示頁面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string keyword, string sort, int pagenumber, int pagesize)
        {

            ViewBag.Keyword = keyword;/*==null?"":keyword ;*/
            ViewBag.Sort = sort;
            ViewBag.Pagenumber = pagenumber;
            ViewBag.PageSize = pagesize;

            return View();
        }




        [Authorize(Roles = "一般員工")]
        public IActionResult Details(int orderId)
        {
            ViewBag.OrderId = orderId;
            ViewBag.Email = dao.GetEmailByOrderId(orderId);

            return View();
        }
        [HttpPost]
        public IActionResult Details(OrderStatusVM vm)
        {
            var query = _context.Orders.Where(O => O.OrderId == vm.OrderId).FirstOrDefault();
            var orderDetails = _context.OrderDetails.Where(o => o.OrderId == vm.OrderId).ToList();
            query.OrderStatusId = vm.StatusId;

            vm.StatusName = _context.OrderStatus.Where(os => os.StatusId == vm.StatusId).Select(vm => vm.StatusName).FirstOrDefault();

            if (vm != null)
            {
                if (vm.StatusName == "已出貨")
                {
                    query.ShippingDate = DateTime.Now;
                    foreach (var item in orderDetails)
                    {
                        var productStockDetail = _context.ProductStockDetails.Where(psd => psd.StockId == item.StockId).FirstOrDefault();
                        productStockDetail.Quantity = productStockDetail.Quantity - item.OrderQuantity;
                        _context.SaveChanges();
                    }
                }
                else if (vm.StatusName == "已完成")
                {
                    query.CompletionDate = DateTime.Now;
                }
                else if (vm.StatusName == "未取貨")
                {
                    foreach (var item in orderDetails)//一張訂單可能有複數商品,單一件商品的資訊要去psd找
                    {
                        var productStockDetail = _context.ProductStockDetails.Where(psd => psd.StockId == item.StockId).FirstOrDefault();
                        productStockDetail.Quantity = productStockDetail.Quantity + item.OrderQuantity;
                        _context.SaveChanges();
                    }
                }
                else if (vm.StatusName == "退款中" && vm.formerStatusName == "已完成")
                {
                    foreach (var item in orderDetails)
                    {
                        var productStockDetail = _context.ProductStockDetails.Where(psd => psd.StockId == item.StockId).FirstOrDefault();
                        productStockDetail.Quantity = productStockDetail.Quantity + item.OrderQuantity;
                        _context.SaveChanges();
                    }
                }
                _context.SaveChanges();
               
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Member")]
        /// <summary>
        /// 寄給會員的order頁面
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult OrderEmailContent(int orderId)
        {
            //要會員登入才能看，且只能看自己的訂單,
            ViewBag.OrderId = orderId;
            var Loginmember = _userInfoService.GetMemberInfo().Email;
            var Ordermember = dao.GetEmailByOrderId(orderId);
            if (Loginmember == Ordermember)
            {
                return View();
            }
            return RedirectToAction("NoRole", "Home");
        }


    }
}
