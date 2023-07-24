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

        public IActionResult changeProductPhoto(int productId, int colorId,int sizeId)
        {
            ProductStockDetails photopath = dao.changeProductPhoto(productId, colorId, sizeId);
            return Json(photopath);
        }
    }
}
