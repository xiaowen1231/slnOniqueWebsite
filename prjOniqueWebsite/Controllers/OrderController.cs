using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.ViewModels;
using prjOniqueWebsite.Models.Daos;

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
        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult Details(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
        
    }
}
