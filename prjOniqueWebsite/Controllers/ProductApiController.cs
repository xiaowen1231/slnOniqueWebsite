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
        public IActionResult GetProductCard()
        {
            ProductRepository repo = new ProductRepository(_context);
            ProductCardDto prodCard = repo.Get();
            return Json(prodCard);
        }
    }
}
