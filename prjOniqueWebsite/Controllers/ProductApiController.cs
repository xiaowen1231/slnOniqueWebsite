using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

namespace prjOniqueWebsite.Controllers
{
    public class ProductApiController : Controller
    {
        private readonly OniqueContext _context;
        public ProductApiController(OniqueContext context)
        {
            _context = context;
        }

        public IActionResult ProductCard()
        {
            ProductCardDto product = (_context.Products
                .Where(p => p.ProductId == 24)
                .Select(p => new ProductCardDto
                {
                    ProductName = p.ProductName,
                    Price = p.Price,
                    PhotoPath = p.PhotoPath
                })).FirstOrDefault();
            if (product == null)
            {
                return null;
            }
            else
            {
                return Json(product);
            }

        }
        public IActionResult NewArrivalsTop4()
        {
            List<ProductCardDto> dto = new ProductDao(_context).NewArrivalsTop4();
            return Json(dto);
        }
    }
}
