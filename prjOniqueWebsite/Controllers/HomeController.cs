using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using prjOniqueWebsite.Models;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OniqueContext _context;
        private readonly UserInfoService _userInfoService;


        public HomeController(OniqueContext context, ILogger<HomeController> logger, UserInfoService userInfoService)
        {
            _logger = logger;
            _context = context;
            _userInfoService = userInfoService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            Dictionary<string, Func<RedirectToActionResult>> roleActions = new Dictionary<string, Func<RedirectToActionResult>>
            {
                {"Member",()=>RedirectToAction("index", "FMember") },
                { "一般員工",()=>RedirectToAction("BackgroundIndex")},
                { "經理",()=>RedirectToAction("BackgroundIndex")}
            };

            foreach(var role in roleActions.Keys)
            {
                if (HttpContext.User.IsInRole(role))
                {
                    return roleActions[role]();
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM vm,string? returnUrl)
        {
            //VM表單驗證
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }

            //todo
            //vm.password 進行雜湊 再去比對

            var member = _context.Members.FirstOrDefault(m => m.Email == vm.Email && m.Password == vm.Password);
            var employee = _context.Employees.FirstOrDefault(e => e.Email == vm.Email && e.Password == vm.Password);

            //輸入錯誤
            if (member == null && employee == null)
            {
                ModelState.AddModelError("", "帳號密碼錯誤!");
                return View(vm);

            }

            //登入
            if (member != null)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,member.MemberId.ToString()),
                    new Claim(ClaimTypes.Role,"Member")
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(identity));

                TempData["AlertLogin"] = member.Name;
            }

            if (employee != null)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,employee.EmployeeId.ToString()),
                };

                string EmployeeLevel = _context.Employees.Where(e=>e.EmployeeId==employee.EmployeeId)
                    .Select(e=>e.PositionNavigation.EmployeeLevelName)
                    .FirstOrDefault();

                claims.Add(new Claim(ClaimTypes.Role, EmployeeLevel));

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                TempData["AlertLogin"] = employee.EmployeeName;

                return RedirectToAction("BackgroundIndex");
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Test()
        {
            return Content(_userInfoService.GetMemberInfo().Name);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult NoLogin()
        {
            return View();
        }

        public IActionResult NoRole()
        {
            return View();
        }

        [Authorize(Roles ="一般員工,經理")]
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