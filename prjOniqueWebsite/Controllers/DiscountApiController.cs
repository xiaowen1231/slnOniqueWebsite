using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class DiscountApiController : Controller
    {
        private DiscountDao _discountDao;
        private OniqueContext _context;
        private IWebHostEnvironment _environment;
        public DiscountApiController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _discountDao = new DiscountDao(_context,_environment);
        }
        public IActionResult GetDiscountList()
        {
            var dto = _discountDao.GetDiscountList();
            return Json(dto);
        }
    }
}
