using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using prjOniqueWebsite.Models;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;
using System;
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
            if (HttpContext.Session.Keys.Contains("Login") && !HttpContext.Session.Keys.Contains("EmployeeLogin"))
                return Content("權限不足");
            if (HttpContext.Session.Keys.Contains("Login"))
                return Content("已登入,會員管理頁面");
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }

            var member = _context.Members.FirstOrDefault(m => m.Email == vm.Email && m.Password == vm.Password);
            var employee = _context.Employees.FirstOrDefault(e => e.Email == vm.Email && e.Password == vm.Password);

            if (member != null)
            {
                string json = JsonSerializer.Serialize(member);
                HttpContext.Session.SetString("Login", json);

                TempData["AlertLogin"] = member.Name;

                return RedirectToAction("index");
            }
            if (employee != null)
            {
                string json = JsonSerializer.Serialize(employee);

                HttpContext.Session.SetString("Login", json);
                HttpContext.Session.SetString("EmployeeLogin", json);

                TempData["AlertLogin"] = employee.EmployeeName;

                return RedirectToAction("BackgroundIndex");
            }
            ModelState.AddModelError("", "帳號密碼錯誤!");
            return View(vm);
        }

        public IActionResult BackgroundIndex()
        {
            return View();
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