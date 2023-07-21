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

        public IActionResult NewArrivalsTop4()
        {
            List<ProductCardDto> dto = new ProductDao(_context).NewArrivalsTop4();
            return Json(dto);
        }
    }
}
