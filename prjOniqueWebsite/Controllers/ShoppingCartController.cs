using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly OniqueContext _context;
        public ShoppingCartController(OniqueContext context)
        {
            _context = context;
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult Index()
        {
            string json = HttpContext.Session.GetString("Login");
            Members members = JsonSerializer.Deserialize<Members>(json);
            var cart = from s in _context.ShoppingCart
                       join psd in _context.ProductStockDetails
                       on s.StockId equals psd.StockId
                       join p in _context.Products
                       on psd.ProductId equals p.ProductId
                       where s.MemberId == members.MemberId
                       select new ShoppingCartDto
                       {
                           Product = p,
                           ProductStock = psd,
                           ShoppingCart = s
                       };
            
            return View(cart.ToList());
        }
    }
}
