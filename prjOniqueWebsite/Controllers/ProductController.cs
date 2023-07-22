using Microsoft.AspNetCore.Mvc;

namespace prjOniqueWebsite.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
