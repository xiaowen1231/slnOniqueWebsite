using Microsoft.AspNetCore.Mvc;

namespace prjOniqueWebsite.Controllers
{
    public class MemberApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
