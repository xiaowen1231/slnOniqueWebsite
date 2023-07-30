using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;

namespace prjOniqueWebsite.Controllers
{
    public class ShoppingCartApiController : Controller
    {
        private readonly OniqueContext _context;
        private readonly ProductDao _dao;
        public ShoppingCartApiController(OniqueContext context)
        {
            _context = context;
            _dao = new ProductDao(_context);
        }

        public IActionResult UpdateOrderQty(int stockId, int orderQty, int shoppingCartId)
        {
            ShoppingCart cart = new ShoppingCartService(_context).UpdateOrderQty(stockId, orderQty, shoppingCartId);
            return Json(cart);
        }
    }
}
