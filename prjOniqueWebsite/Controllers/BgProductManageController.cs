using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class BgProductManageController : Controller
    {

        private readonly OniqueContext _context;
        public BgProductManageController(OniqueContext context ) 
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductEdit(int productId)
        {
            var query = from p in _context.Products
                        join c in _context.Categories
                        on p.ProductCategoryId equals c.CategoryId
                        join s in _context.Supplier
                        on p.SupplierId equals s.SupplierId
                        where p.ProductId==productId
                        select new BgEditProductDto
                        {                            
                            ProductName = p.ProductName,
                            Price = p.Price,
                            PhotoPath = p.PhotoPath,
                            Category = c.CategoryName,
                            SupplierName = s.SupplierName,
                            AddedTime = p.AddedTime,
                            ShelfTime = p.ShelfTime,
                            Description = p.Description,
                        };
            BgEditProductDto dto = query.FirstOrDefault();
            
                return View(dto);
            
        }
    }
}
