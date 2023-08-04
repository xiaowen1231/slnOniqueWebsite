using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class FMemberController : Controller
    {
        public IActionResult Index( string display)
        {
            ViewBag.Display = display;
            return View();
        }
        public IActionResult MemberInfo()
        {
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
