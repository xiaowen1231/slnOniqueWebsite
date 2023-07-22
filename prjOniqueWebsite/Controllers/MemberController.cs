using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class MemberController : Controller
    {
        private readonly OniqueContext _context;
        public MemberController(OniqueContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
