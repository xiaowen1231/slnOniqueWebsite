using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Data;
using prjOniqueWebsite.Models.Infra;
using static prjOniqueWebsite.Controllers.ProductApiController;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Hosting;

namespace prjOniqueWebsite.Controllers
{
    public class OrderApiController : Controller
    {

        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly OniqueContext _context;
        private IWebHostEnvironment _enviroment;

        OrderDao dao = null;
        public OrderApiController(OniqueContext context, IUrlHelperFactory urlHelperFactory, IWebHostEnvironment enviroment)
        {
            _context = context;
            dao = new OrderDao(_context);
            _urlHelperFactory = urlHelperFactory;
            _enviroment = enviroment;
        }
        public IActionResult SendOrderHtml(string OrderId, string Email, string statusNow, string Name)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            string pathToFile = Path.Combine(_enviroment.WebRootPath, "Template", "email.html");

            string TemplateContent = "";
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))//讀出html內容
            {
                TemplateContent = SourceReader.ReadToEnd();
            }
            TemplateContent = string.Format(TemplateContent,
                
                "fakedata1",
                "fakedata2",
                "fakedata3",
                "fakedata4",
                "fakedata5",
                "fakedata6",
                "fakedata7",
                "fakedata8",
                "fakedata9",
                "fakedata10",
                "fakedata11",
                "fakedata12",
                "fakedata13",
                "fakedata14",
                "fakedata15",
                "fakedata16",
                "fakedata17",
                "fakedata18",
                "fakedata19"


                );
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = TemplateContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(Email);
            smtpClient.Send(mailMessage);

            return Content("");
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        ///依據回傳的資料更新畫面
        ///資料和分頁籤
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        public IActionResult OrderList(string keyword, string sort, int pagenumber, int pagesize, string? startDate)
        {
            DateTime? parsedDateTime = null;
            if (!string.IsNullOrEmpty(startDate))
            {
                parsedDateTime = DateTime.Parse(startDate);
            }

            var dto = dao.SearchOrderList(keyword, sort, parsedDateTime);
            var index = OrderListIndex(dto, pagenumber, pagesize);
            return Json(index);
        }
        /// <summary>
        /// 依據回傳的資料與分頁大小製作分頁標籤
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        public IActionResult OrderListIndex(List<OrderListDto> order, int pagenumber, int pagesize)
        {
            var data = new OrderGUIdto();

            var criteria = prepareCriteria(pagenumber);
            criteria.PageSize = pagesize;
            data.Criteria = criteria;

            int totalOrderCount = order.Count();
            data.OrderPaginationInfo = new OrderPaginationInfo(totalOrderCount, criteria.PageSize, criteria.PageNumber, $"/OrderApi/OrderList?pagenumber={pagenumber}&pagesize={pagesize}");
            data.Orders = order.Skip(criteria.recordStartIndex).Take(criteria.PageSize).ToList();
            return Json(data);
        }

        public class OrderCriteria
        {
            private int _PageNumber = 1;

            public int PageNumber
            {
                get { return _PageNumber; }
                set { _PageNumber = value < 1 ? 1 : value; }
            }

            private int _PageSize = 10;

            public int PageSize
            {
                get { return _PageSize; }
                set { _PageSize = value < 1 ? 10 : value; }
            }

            public int recordStartIndex
            {
                get { return (PageNumber - 1) * PageSize; }
            }
        }

        public OrderCriteria prepareCriteria(int pageNumber)
        {
            var result = new OrderCriteria();
            result.PageNumber = pageNumber;

            return result;
        }
        public class OrderGUIdto
        {
            public OrderCriteria Criteria { get; set; }
            public OrderPaginationInfo OrderPaginationInfo { get; set; }
            public List<OrderListDto> Orders { get; set; }

        }
        public class OrderPaginationInfo
        {
            public OrderPaginationInfo(int totalRecords, int pageSize, int pageNumber, string urlTemplate)
            {
                TotalRecords = totalRecords;
                PageSize = pageSize;
                PageNumber = pageNumber;
                this.urlTemplate = urlTemplate;
            }
            private string urlTemplate;
            public string GetUrl(int pageNumber)
            {
                return string.Format(urlTemplate, pageNumber);
            }

            public int TotalRecords { get; set; }

            public int PageSize { get; set; }

            public int PageNumber { get; set; }

            public int Pages => (int)Math.Ceiling((double)TotalRecords / PageSize);

            public int PageItemCount => 5;

            public int PageBarStartNumber
            {
                get
                {
                    int startNumber = PageNumber - ((int)Math.Floor((double)this.PageItemCount / 2));
                    return startNumber < 1 ? 1 : startNumber;
                }
            }

            public int PageItemPrevNumber => (PageBarStartNumber <= 1) ? 1 : PageBarStartNumber - 1;

            public int PageBarItemCount => PageBarStartNumber + PageItemCount > Pages
                ? Pages - PageBarStartNumber + 1
                : PageItemCount;
            public int PageItemNextNumber => (PageBarStartNumber + PageItemCount >= Pages) ? Pages : PageBarStartNumber + PageItemCount;
        }



        public IActionResult orderProductDetail(string orderId)
        {

            List<OrderProductsListDto> dto = dao.getProductDetail(orderId);
            return Json(dto);
        }
        public IActionResult orderShippingDetail(string orderId)
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
        public IActionResult OrderStatusNow(string orderId)
        {
            OrderStatusDto dto = dao.GetOrderStatus(orderId);
            return Json(dto);
        }
        public IActionResult tags()
        {
            var tag = _context.Members.Select(c => c.Name).ToList();
            return Json(tag);
        }
        public IActionResult getOrderStatusCount(int statusId)
        {
            var count = dao.GetOrderStatusCount(statusId); return Json(count);
        }
        /// <summary>
        /// 依照orderId找出目前的statusName，再給予依照商業邏輯正確的statusName的選擇，傳回的是選項，不是order的資料
        /// 要把pay的方式列入考慮
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public IActionResult GetOrderStatusOptions(string OrderId)
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
                                  StatusId = os.StatusId,
                                  OrderDate = o.OrderDate,
                              };
            var query = _context.OrderStatus;

            List<OrderStatus> data = null;


            if (statusNow == "待出貨")
            {
                data = query.Where(c => c.StatusName == "已出貨" || c.StatusName == "已取消").ToList();
            }
            if (statusNow == "已出貨")
            {
                data = query.Where(c => c.StatusName == "未取貨" || c.StatusName == "已完成").ToList();
            }


            if (statusNow == "已完成")//如果訂單完成小於7天才有這選項
            {
                data = query.Where(c => c.StatusName == "退款中").ToList();
            }
            if (statusNow == "已取消" && paymentMethodNow != "貨到付款")
            {
                data = query.Where(c => c.StatusName == "退款中").ToList();
            }

            if (statusNow == "已取消" && paymentMethodNow == "貨到付款")
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

        /// <summary>
        /// 寄信
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="Email"></param>
        /// <param name="statusNow"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SendMail(string OrderId, string Email, string statusNow, string Name)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("msit147onique@gmail.com", "piukwngszjdyzmov"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };


            var urlHelper = _urlHelperFactory.GetUrlHelper(ControllerContext);


            string orderDetailsUrl = urlHelper.Action("OrderEmailContent", "Order", new { OrderId }, HttpContext.Request.Scheme);


            string htmlStr =
                $@"
    <div style=""font-size:24px"">
        <div>親愛的{Name}您好!</div>
        <div>您的訂單 編號:{OrderId}</div>
        <div>您的訂單狀態目前已更新為:{statusNow}</div>
        <div><a href=""{orderDetailsUrl}"">點擊查看訂單狀態</a></div>
    </div>";
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("msit147onique@gmail.com"),
                Subject = "訂單內容",
                Body = htmlStr,
                IsBodyHtml = true
            };

            mailMessage.To.Add(Email);
            smtpClient.Send(mailMessage);

            return Content("寄送簡訊完成");
        }


        public IActionResult DeleteOrder(string OrderId)
        {
            ApiResult result = new ApiResult();
            try
            {
                dao.DeleteOrder(OrderId);
                result.StatusCode = 200;
                result.StatusMessage = "刪除訂單成功";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.StatusMessage = "刪除訂單失敗";
            }
            return Json(result);
        }

        public  void SendHtmlEmail(string OrderId)
        {
            var dto=dao.getEmailTemplateContent(OrderId);
            new SendHtmlEmail().SendOrderHtml(dto, _enviroment,_urlHelperFactory,ControllerContext,HttpContext);

        }
    }
}
