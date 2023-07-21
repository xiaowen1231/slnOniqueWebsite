using Microsoft.AspNetCore.Mvc;

namespace prjOniqueWebsite.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
