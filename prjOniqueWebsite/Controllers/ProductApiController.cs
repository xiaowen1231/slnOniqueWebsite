using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

namespace prjOniqueWebsite.Controllers
{
    public class ProductApiController : Controller
    {
        private readonly ProductDao _productDao = null;
        private readonly OniqueContext _context;
        public ProductApiController(OniqueContext context, ProductDao productDao)
        {
            _context = context;
            _productDao = new ProductDao(_context);
        }

        public IActionResult NewArrivalsTop4()
        {
            
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
            if(product == null)
            {
                return null;
            }
            else
            {
                return Json(product);
            }
            
        }
    }
}
