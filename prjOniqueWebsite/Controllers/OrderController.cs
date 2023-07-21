using Microsoft.AspNetCore.Mvc;

namespace prjOniqueWebsite.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details()
        {
            return PartialView();
        }
    }
}
