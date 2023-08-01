using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.ViewModels;
using prjOniqueWebsite.Models.Daos;
using System.Drawing.Printing;
using Humanizer.Localisation.TimeToClockNotation;

namespace prjOniqueWebsite.Controllers
{
    public class OrderController : Controller
    {
        private readonly OniqueContext _context;
        OrderDao dao = null;
        public OrderController(OniqueContext context)
        {
            _context = context;
            dao = new OrderDao(_context);
        }
        public IActionResult Index(int page=1,int pageSize=10)
        {
            OrderPagination pagination = new OrderPagination();
            pagination.PageSize = pageSize;
            pagination.PageIndex = page;
            pagination.OrderCount = _context.Orders.Count();
            pagination.TotalPages = (int)Math.Ceiling((double)pagination.OrderCount / pageSize);
            int startIndex = pageSize * (page - 1);
            pagination.Orders=dao.getOrderList().OrderBy(c=>c.OrderId).Skip(startIndex).Take(pageSize).ToList();
            //每一頁的內容物
                     
            return View(pagination);
        }
        public IActionResult Details(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
        [HttpPost]
        public IActionResult Details(OrderStatusVM vm)
        {
            var query = _context.Orders.FirstOrDefault(o => o.OrderId == vm.OrderId);

            query.OrderStatusId = vm.StatusId;

            vm.StatusName = _context.OrderStatus.Where(os => os.StatusId == vm.StatusId).Select(vm => vm.StatusName).FirstOrDefault();




            if (vm != null)
            {
                if (vm.StatusName == "已出貨")
                {
                    query.ShippingDate = DateTime.Now;
                }
                else if (vm.StatusName == "已完成")
                {
                    query.CompletionDate = DateTime.Now;
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }



    }
}
