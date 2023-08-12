using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;

namespace prjOniqueWebsite.Controllers
{
    public class DiscountManageApiController : Controller
    {

        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        private ProductDao _productDao;
        private DiscountService _service;

        public DiscountManageApiController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _productDao = new ProductDao(_context);
            _service = new DiscountService(_context, _environment);
        }

        public IActionResult DisplayProductList(string keyword)
        {
            var product = _productDao.SearchProductList(keyword, "", "");
            return Json(product);
        }

        [HttpPost]
        public IActionResult AddToDiscount(int productId, int discountId)
        {
            try
            {
                _service.AddToDiscount(productId, discountId);
                ApiResult result = new ApiResult
                {
                    StatusCode = 200,
                    StatusMessage = "加入優惠商品成功!"
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                ApiResult result = new ApiResult
                {
                    StatusCode = 500,
                    StatusMessage = "加入優惠商品失敗!" + ex.Message
                };
                return Json(result);
            }
        }
    }
}
