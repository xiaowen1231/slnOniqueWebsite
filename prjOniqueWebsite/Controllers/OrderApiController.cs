using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Data;
using static prjOniqueWebsite.Controllers.ProductApiController;

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
        /// <summary>
        ///依據回傳的資料更新畫面
        ///資料和分頁籤
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="sort"></param>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        public IActionResult OrderList(string keyword,string sort,int pagenumber,int pagesize)
        {
            var dto = dao.SearchOrderList(keyword, sort);
            var index= OrderListIndex(dto, pagenumber,pagesize);
            return Json(index);
        }
        /// <summary>
        /// 依據回傳的資料與分頁大小製作分頁標籤
        /// </summary>
        /// <param name="order"></param>
        /// <param name="pagenumber"></param>
        /// <returns></returns>
        public IActionResult OrderListIndex(List<OrderListDto>order,int pagenumber,int pagesize)
        {
            var data = new GUIdto();
            var criteria = prepareCriteria(pagenumber,pagesize);
            criteria.PageSize = pagesize;
            data.Criteria = criteria;

            int totalOrderCount=order.Count();
            data.Pagination=new PaginationInfo(totalOrderCount, pagesize, criteria.PageNumber,$"/OrderApi/OrderList?pagenumber={pagenumber}&pagesize={pagesize}");
            data.orders=order.Skip(criteria.recordStartIndex).Take(criteria.PageSize).ToList();
            return Json(data);
        }

        public class Criteria
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

        public Criteria prepareCriteria(int pageNumber,int pagesize)
        {
            var result = new Criteria();
            result.PageNumber = pageNumber;
            result.PageSize = pagesize;
            return result;
        }
        public class GUIdto
        {
            public Criteria Criteria { get; set; }
            public PaginationInfo Pagination { get; set; }
            public List<OrderListDto> orders{ get; set; }

        }
        public class PaginationInfo
        {
            public PaginationInfo(int totalRecords, int pageSize, int pageNumber, string urlTemplate)
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
        //public IActionResult showOrderList()
        //{
        //    List<OrderListDto> dto = dao.getOrderList().ToList();
        //    return Json(dto);
        //}
        /// <summary>
        /// 傳分頁資料給前台OrderIndex更新頁面資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort">分類</param>
        /// <returns>List<OrderListDto> dto</returns>
        //public IActionResult orderPage(int page, int pageSize, string? sort)
        //{
        //    List<OrderListDto> dto = null;
        //    switch (sort)
        //    {
        //        case "OrderDateAscending":
        //            dto = dao.getOrderList().OrderBy(order => order.OrderDate)
        //            .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        case "OrderDateDescending":
        //            dto = dao.getOrderList().OrderByDescending(order => order.OrderDate)
        //            .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        default:
        //            dto = dao.getOrderList().OrderByDescending(order => order.OrderDate)
        //            .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //    }

        //    return Json(dto);
        //}
        //public IActionResult search(string? keyWord,int page,int pageSize,string? sort)
        //{
        //    IEnumerable<OrderListDto> data = null;

        //    var query = from o in _context.Orders
        //                join os in _context.OrderStatus
        //                on o.OrderStatusId equals os.StatusId
        //                join m in _context.Members
        //                on o.MemberId equals m.MemberId
        //                join pm in _context.PaymentMethods
        //                on o.PaymentMethodId equals pm.PaymentMethodId
        //                select new OrderListDto
        //                {
        //                    StatusName = os.StatusName,
        //                    OrderId = o.OrderId,
        //                    Name = m.Name,
        //                    OrderDate = o.OrderDate,
        //                    ShippingDate = (DateTime)o.ShippingDate,
        //                    PaymentMethodName = pm.PaymentMethodName,
        //                    PhotoPath = m.PhotoPath
        //                };
        //    if (string.IsNullOrEmpty(keyWord))
        //    {
        //        switch (sort)
        //        {

        //            case "OrderDateAscending":
        //                data = query.OrderBy(order => order.OrderDate)
        //                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //                break;
        //            case "OrderDateDescending":
        //                data =query.OrderByDescending(order => order.OrderDate)
        //                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //                break;
        //            default:
        //                data = query.OrderByDescending(order => order.OrderDate)
        //                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        data = query.Where(o => o.Name.Contains(keyWord) || o.StatusName.Contains(keyWord) || o.OrderId.ToString().Contains(keyWord))
        //    }



        //    return Json(data);
        //}


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

            List<OrderStatus> data = null;


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
    }
}
