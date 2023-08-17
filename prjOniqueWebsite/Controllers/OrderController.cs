using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.ViewModels;
using prjOniqueWebsite.Models.Daos;
using System.Drawing.Printing;
using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Authorization;
using prjOniqueWebsite.Models.Infra;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Routing;
using prjOniqueWebsite.Models.Dtos;

namespace prjOniqueWebsite.Controllers
{
    public class OrderController : Controller
    {
        private readonly OniqueContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHostEnvironment _environment;

        OrderDao dao = null;
        public OrderController(OniqueContext context, UserInfoService userInfoService,IUrlHelperFactory urlHelperFactory, IWebHostEnvironment environment)
        {
            _context = context;
            dao = new OrderDao(_context);
            _userInfoService = userInfoService;
            _urlHelperFactory = urlHelperFactory;
            _environment = environment;
            
        }

        [Authorize(Roles = "一般員工,經理")]
        /// <summary>
        /// orderList的展示頁面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(string keyword, string sort, int pagenumber, int pagesize,string startDate ,string seletedValue)
        {
            try
            {

            ViewBag.Keyword = keyword;
            ViewBag.Sort = sort;
            ViewBag.Pagenumber = pagenumber;
            ViewBag.PageSize = pagesize;
            ViewBag.StartDate = startDate;
            ViewBag.SeletedValue = seletedValue;
            return View();
            }catch (Exception ex)
            {
                return RedirectToAction("Index", "Order");
            }

        }




        [Authorize(Roles = "一般員工,經理")]
        public IActionResult Details(string orderId)
        {
            try
            {

            ViewBag.OrderId = orderId;
            ViewBag.Email = dao.GetEmailByOrderId(orderId);

            return View();
            }catch (Exception ex)
            {
                return RedirectToAction("Index", "Order");
            }
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
                var dto = dao.getEmailTemplateContent(query.OrderId);

                new SendHtmlEmail().SendOrderHtml(dto, _environment,_urlHelperFactory,ControllerContext,HttpContext);

                //var dto = from o in _context.Orders
                //          where o.OrderId == query.OrderId
                //          join m in _context.Members
                //          on o.MemberId equals m.MemberId
                //          join os in _context.OrderStatus
                //          on o.OrderStatusId equals os.StatusId
                //          select new SendMailDto
                //          {
                //              OrderId = o.OrderId,
                //              Email = m.Email,
                //              StatusNow = os.StatusName,
                //              Name = m.Name,
                //          };
                //new SendEmail().SendMail(dto.FirstOrDefault(), _urlHelperFactory, ControllerContext, HttpContext);
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Member")]
        /// <summary>
        /// 寄給會員的order頁面
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public IActionResult OrderEmailContent(string orderId)
        {
            try
            {

            //要會員登入才能看，且只能看自己的訂單,
            ViewBag.OrderId = orderId;
            var Loginmember = _userInfoService.GetMemberInfo().Email;//目前登入者的資料
            var Ordermember = dao.GetEmailByOrderId(orderId);//此訂單對應客戶帳號
            if (Loginmember == Ordermember)//比對兩者
            {
                return View();
            }
            return RedirectToAction("NoRole", "Home");
            }
            catch (Exception ex)
            {
            return RedirectToAction("NoRole", "Home");

            }
        }

        
    }
}
