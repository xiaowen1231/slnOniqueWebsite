using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;

namespace prjOniqueWebsite.Controllers
{
    public class BgProductManageApiController : Controller
    {
        private readonly OniqueContext _context;
        private readonly ProductDao _dao;
        private readonly IWebHostEnvironment _environment;
        public BgProductManageApiController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _dao = new ProductDao(_context);
            _environment = environment;
        }
        public IActionResult Index()/*vm*/
        {
            //if (@ModelState.IsValid){
            //    "編輯商品名稱不能一樣"
            //        retrun view(vm)
            //}
            return View();
        }
        public IActionResult showProductName()
        {
            var query = from p in _context.Products
                        select new Products
                        {
                            ProductId = p.ProductId,
                            ProductName = p.ProductName
                        };
            return View(query);
        }
        public IActionResult ShowColorList()
        {
            var query = from c in _context.ProductColors
                        select new ProductColors
                        {
                            ColorId = c.ColorId,
                            ColorName = c.ColorName
                        };
            List<ProductColors> dto = query.ToList();
            return Json(dto);
        }
        public IActionResult ShowCategoryList()
        {
            var query = from c in _context.Categories
                        select c;
            return Json(query);
        }
        public IActionResult ShowSupplierList()
        {
            var query = from s in _context.Supplier
                        select s;
            return Json(query);
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

        public IActionResult DeleteStock(int id)
        {

            var result = new ApiResult();
            try
            {

                var psd = _context.ProductStockDetails.FirstOrDefault(psd => psd.StockId == id);

                if (psd == null)
                {
                    result.StatusCode = 500;
                    result.StatusMessage = "無此商品庫存設定，請重新確認";
                    return Json(result);
                }
                else
                {
                    _context.ProductStockDetails.Remove(psd);
                    _context.SaveChanges();
                    result.StatusCode = 200;
                    result.StatusMessage = "刪除成功!";
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.StatusMessage = ex.Message;
                return Json(result);
            }


        }

    }
}
