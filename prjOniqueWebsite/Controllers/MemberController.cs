using Microsoft.AspNetCore.Mvc;

namespace prjOniqueWebsite.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
