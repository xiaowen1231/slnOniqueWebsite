using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class ProductApiController : Controller
    {
        private readonly OniqueContext _context;
        ProductDao dao = null;

        public ProductApiController(OniqueContext context)
        {
            _context = context;
            dao = new ProductDao(_context);
        }

        public IActionResult NewArrivalsTop4()
        {
            List<ProductCardDto> dto = dao.NewArrivalsTop4();
            return Json(dto);
        }

        public IActionResult HotTop4()
        {
            List<ProductCardDto> dto = dao.HotTop4();
            return Json(dto);
        }

        public IActionResult GetStockSize(int id,int colorId)
        {
            List<ProductSizes> sizes = dao.GetStockSize(id, colorId);
            return Json(sizes);
        }

        public IActionResult GetStockColor(int id)
        {
            List<ProductColors> colors = dao.GetStockColor(id);
            return Json(colors);
        }

        public IActionResult GetStockDetail(int productId, int colorId,int sizeId)
        {
            ProductStockDetails stock = dao.GetStockDetail(productId, colorId, sizeId);
            return Json(stock);
        }

        public IActionResult AddToCart(int stockId,int qty)
        {
            string json = HttpContext.Session.GetString("Login");

            Members member = JsonSerializer.Deserialize<Members>(json);

            dao.AddToCart(stockId,qty,member);

            var cart = dao.CartItems(member);
            
            return Json(cart.Count);
        }
        public IActionResult CartItems()
        {
            string json = HttpContext.Session.GetString("Login");

            Members member = JsonSerializer.Deserialize<Members>(json);
            
            var cart = dao.CartItems(member);

            return Json(cart.Count);
        }
        public IActionResult CartList()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            var cart = dao.CartItems(member);
            
            return Json(cart);
        }
    }
}
