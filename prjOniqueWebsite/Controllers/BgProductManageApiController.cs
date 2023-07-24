using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class BgProductManageApiController : Controller
    {
        private readonly OniqueContext _context;
        public BgProductManageApiController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowBgProductManageList()
        {

            var query = from p in _context.Products
                        join c in _context.Categories
                        on p.ProductCategoryId equals c.CategoryId
                        select new BgProductManageListDto
                        {
                            ProductName = p.ProductName,
                            PhotoPath = p.PhotoPath,
                            Price = p.Price,
                            Category = c.CategoryName,                            
                        };

            List<BgProductManageListDto> dto = query.ToList();


            return Json(dto);
        }
    }
}
