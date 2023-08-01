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
        public IActionResult Index()/*vm*/
        {
            //if (@ModelState.IsValid){
            //    "編輯商品名稱不能一樣"
            //        retrun view(vm)
            //}
            return View();
        }
        public IActionResult ShowColorList()
        {
            var query = from c in _context.ProductColors
                        select new ProductColors
                        {
                            ColorId= c.ColorId,
                            ColorName = c.ColorName
                        };
            List<ProductColors> dto = query.ToList();
            return Json(dto);
        }
        public IActionResult ShowSizeList()
        {
            var query = from s in _context.ProductSizes
                        select new ProductSizes
                        {
                            SizeId = s.SizeId,
                            SizeName = s.SizeName
                        };
            List<ProductSizes> dto = query.ToList();
            return Json(dto);
        }
        public IActionResult LoadColor()
        {
            var Color = from c in _context.ProductColors
                        select c;
            return Json(Color);
        }
        public IActionResult LoadSize()
        {
            var Size = from s in _context.ProductSizes
                       select s;
            return Json(Size);
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
        public IActionResult LoadDiscountList()
        {
            var Discount = from d in _context.Discounts
                           select d;
            return Json(Discount);
        }
    }
}
