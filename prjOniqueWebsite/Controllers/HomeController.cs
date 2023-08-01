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
            TempData["AlertLogin"]=member.Name;
            return RedirectToAction("index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            MemberVM vm = (from m in _context.Members
                           join c in _context.Citys
                           on m.Citys equals c.CityId
                           join a in _context.Areas
                           on m.Areas equals a.AreaId
                           join ml in _context.MemberLevel
                           on m.MemberLevel equals ml.MemberLevelId
                           select new MemberVM
                           {
                               MemberLevel = ml.MemberLevelName,
                               Citys = c.CityName,
                               Areas = a.AreaName
                           }).FirstOrDefault();
            ViewBag.memberlevel = vm.MemberLevel;
            ViewBag.city = vm.Citys;
            ViewBag.area = vm.Areas;
            return View();
        }
        [HttpPost]
        public IActionResult Register(MemberVM vm)
        {
            var mem = new Members()
            {
                //string fileName = $"MemberId_{member.MemberId}.jpg";
                Name = vm.Name,
                Password = vm.Password,
                Email = vm.Email,
                Phone = vm.Phone,
                Gender = Convert.ToBoolean(vm.Gender),
                Citys = Convert.ToInt32(vm.Citys),
                Areas = Convert.ToInt32(vm.Areas),
                Address = vm.Address,
                MemberLevel = 1,
                RegisterDate = DateTime.Now,
                DateOfBirth = Convert.ToDateTime(vm.DateOfBirth),
            };


            _context.Members.Add(mem);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}