using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class EmployeeApiController : Controller
    {
        private readonly OniqueContext _context;
        public EmployeeApiController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
