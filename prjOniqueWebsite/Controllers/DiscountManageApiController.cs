using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
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
        private DiscountDao _discountDao;

        public DiscountManageApiController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _productDao = new ProductDao(_context);
            _service = new DiscountService(_context, _environment);
            _discountDao = new DiscountDao(_context, _environment);
        }

        public IActionResult DisplayProductList(string keyword)
        {
            var product = _productDao.SearchProductList(keyword, "", "",0);
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

        public IActionResult DisplayDiscountProductList(int discountId)
        {
            var datas = _discountDao.GetDiscountProducts(discountId);
            return Json(datas);
        }

        public IActionResult DeleteDiscountProduct(int id)
        {
            try
            {
                _service.RemoveDiscountProuct(id);
                var result = new ApiResult
                {
                    StatusCode = 200,
                    StatusMessage = "移除優惠活動成功!"
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new ApiResult
                {
                    StatusCode = 500,
                    StatusMessage = "移除失敗!" + ex.Message
                };
                return Json(result);
            }

        }

        public IActionResult DeleteDiscount(int id)
        {
            try
            {
                _discountDao.DeleteDiscount(id);
                var result = new ApiResult
                {
                    StatusCode = 200,
                    StatusMessage = "刪除優惠成功!"
                };
                return Json(result);
            }catch
            {
                var result = new ApiResult
                {
                    StatusCode = 500,
                    StatusMessage = "刪除優惠失敗!"
                };
                return Json(result);
            }
        }
    }
}
