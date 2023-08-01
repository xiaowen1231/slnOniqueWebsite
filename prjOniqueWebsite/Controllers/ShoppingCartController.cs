using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Repositories;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly OniqueContext _context;
        public readonly ProductDao _dao;
        public ShoppingCartController(OniqueContext context)
        {
            _context = context;
            _dao = new ProductDao(context);
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult Index()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            List<ShoppingCartDto> cart = _dao.CartItems(member);
            return View(cart);
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult OrderConfirmation()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            return View(member);
        }

    }
}
