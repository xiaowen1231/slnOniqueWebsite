using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Infra;

namespace prjOniqueWebsite.Controllers
{
    public class ShoppingCartController : Controller
    {
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
