using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class MemberApiController : Controller
    {
        private readonly OniqueContext _context;
        public MemberApiController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
