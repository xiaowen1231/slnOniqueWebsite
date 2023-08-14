using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;

namespace prjOniqueWebsite.Controllers
{
    public class ProductController : Controller
    {
        private readonly OniqueContext _context;

        public ProductController(OniqueContext context)
        {
            _context = context;
        }

        public IActionResult List(string keyword, string categoryName, string rank, int pageNumber, int? discountId)
        {
            ViewBag.CategoryName = categoryName;
            ViewBag.Keyword = keyword;
            ViewBag.PageNumber = pageNumber;
            ViewBag.DiscountId = discountId == null ? 0:discountId ;
            ViewBag.HasProduct = false;
            if (discountId != null)
            {
                ViewBag.HasProduct = _context.Products.Any(p => p.DiscountId == discountId);
                ViewBag.DiscountName = _context.Discounts.Where(d => d.Id == discountId).Select(d => d.Title).FirstOrDefault();
                ViewBag.PhotoPath = _context.Discounts.Where(d => d.Id == discountId).Select(d => d.PhotoPath).FirstOrDefault();
            }
            ViewBag.Rank = rank;
            return View();
        }

        public IActionResult Detail(int id)
        {
            try
            {
                var dto = new ProductService(_context).ProductInfo(id);
                return View(dto);
            }
            catch
            {
                return RedirectToAction("NotReady");
            }
        }

        public IActionResult NotReady()
        {
            return View();
        }
    }
}
