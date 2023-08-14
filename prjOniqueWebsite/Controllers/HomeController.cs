using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NuGet.Common;
using prjOniqueWebsite.Models;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;
using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OniqueContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao;
        MemberService _service;

        public HomeController(OniqueContext context, ILogger<HomeController> logger, UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
            _service = new MemberService(_context, _environment);
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

            foreach (var role in roleActions.Keys)
            {
                if (HttpContext.User.IsInRole(role))
                {
                    return roleActions[role]();
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM vm, string? returnUrl)
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
                    new Claim("MemberId",member.MemberId.ToString()),
                    new Claim(ClaimTypes.Role,"Member"),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                TempData["AlertLogin"] = member.Name;
            }

            if (employee != null)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim("EmployeeId",employee.EmployeeId.ToString()),
                };

                string EmployeeLevel = _context.Employees.Where(e => e.EmployeeId == employee.EmployeeId)
                    .Select(e => e.PositionNavigation.EmployeeLevelName)
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

        [Authorize(Roles = "一般員工,經理")]
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
            return View();
        }

        [HttpPost]
        public IActionResult Register(FMemberVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                _service.MemberRegister(vm);
                TempData["AlertRegister"] = vm.Name;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","新增失敗，"+ex.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }
    }

}