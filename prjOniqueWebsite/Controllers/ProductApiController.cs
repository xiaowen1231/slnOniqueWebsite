using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

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

        public bool isLogin()
        {
            if (HttpContext.Session.Keys.Contains("test"))
                return true;
            return false;
        }

        //public IActionResult AddToCart()
        //{
        //    ;
        //}
    }
}
