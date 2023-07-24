using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly OniqueContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(OniqueContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.Keys.Contains("Login")) 
                return Content("已登入,會員管理頁面");
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM vm)
        {

            var member = _context.Members.FirstOrDefault(m=>m.Email == vm.Email&&m.Password==vm.Password);
            if(member == null)
                return View();
            string json = JsonSerializer.Serialize(member);
            HttpContext.Session.SetString("Login", json);
            return RedirectToAction("index");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}