using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class FMemberController : Controller
    {
        private readonly OniqueContext _context;
        public FMemberController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index( string display)
        {
            ViewBag.Display = display;
            return View();
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult MemberInfo()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            var mem = (from m in _context.Members
                       join 
                       )
            return PartialView();
        }
        public IActionResult MemberOrder()
        {
            return PartialView();
        }
        public IActionResult MemberMyKeep()
        {
            return PartialView();
        }
        public IActionResult MemberPassword()
        {
            return PartialView();
        }
    }
}
