using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NuGet.Common;
using prjOniqueWebsite.Models;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;
using System;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;
using static prjOniqueWebsite.Models.Infra.LineLogin;

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


        public ActionResult LineLoginDirect()
        {
            string response_type = "code";
            string client_id = "2000432178";
            string redirect_uri = HttpUtility.UrlEncode("https://localhost:7056/Home/callback");
            string state = "123";
            string LineLoginUrl = string.Format("https://access.line.me/oauth2/v2.1/authorize?response_type={0}&client_id={1}&redirect_uri={2}&state={3}&scope=openid%20profile&nonce=09876xyz",
                response_type,
                client_id,
                redirect_uri,
                state
                );
            return Redirect(LineLoginUrl);
        }


        public ActionResult callback(string code, string state)
        {
            if (state == "123")
            {
                #region Api變數宣告
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string result = string.Empty;
                NameValueCollection nvc = new NameValueCollection();
                #endregion
                try
                {
                    //取回Token
                    string ApiUrl_Token = "https://api.line.me/oauth2/v2.1/token";
                    nvc.Add("grant_type", "authorization_code");
                    nvc.Add("code", code);
                    nvc.Add("redirect_uri", "https://localhost:7056/Home/callback");
                    nvc.Add("client_id", "2000432178");
                    nvc.Add("client_secret", "f2ff2eb09067aa2a3893a3fca63dfeff");
                    string JsonStr = Encoding.UTF8.GetString(wc.UploadValues(ApiUrl_Token, "POST", nvc));
                    LineLogin.LineLoginToken ToKenObj = JsonConvert.DeserializeObject<LineLogin.LineLoginToken>(JsonStr);
                    wc.Headers.Clear();

                    //取回User Profile
                    string ApiUrl_Profile = "https://api.line.me/v2/profile";
                    wc.Headers.Add("Authorization", "Bearer " + ToKenObj.access_token);
                    string UserProfile = wc.DownloadString(ApiUrl_Profile);
                    LineLogin.LineProfile ProfileObj = JsonConvert.DeserializeObject<LineLogin.LineProfile>(UserProfile);

                    return RedirectToAction("LineLogin", "Home", ProfileObj);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    throw;
                }
            }
            return View();
        }


        public IActionResult LineLogin(LineLogin.LineProfile lineDto)
        {
            var memberInDb = _context.Members.FirstOrDefault(m => m.LineUserId == lineDto.userId);

            if (memberInDb == null)
            {

                TempData["LineUserId"] = lineDto.userId;
                TempData["LineUserName"] = lineDto.displayName;
                TempData["LineUserPictureUrl"] = lineDto.pictureUrl;

                return RedirectToAction("LineRegister", "Home");
            }else 
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim("MemberId",memberInDb.MemberId.ToString()),
                    new Claim(ClaimTypes.Role,"Member"),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                TempData["AlertLogin"] = memberInDb.Name;

                return RedirectToAction("Index");
            }
;
        }

        public IActionResult LineRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LineRegister(LineRegisterVM vm)
        {
            LineLogin.LineProfile lineProfile = new LineLogin.LineProfile();


            lineProfile.userId = TempData["LineUserId"].ToString();
            lineProfile.displayName = TempData["LineUserName"].ToString();
            lineProfile.pictureUrl = TempData["LineUserPictureUrl"].ToString();

            if (ModelState.IsValid == false)
            {

                TempData["LineUserId"] = lineProfile.userId;
                TempData["LineUserName"] = lineProfile.displayName;
                TempData["LineUserPictureUrl"] = lineProfile.pictureUrl;

                return View(vm);
            }
            try
            {

                var mem = _service.LineRegister(vm,lineProfile) ;

                List<Claim> claims = new List<Claim>
                {
                    new Claim("MemberId",mem.MemberId.ToString()),
                    new Claim(ClaimTypes.Role,"Member"),
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                TempData["AlertLogin"] = mem.Name;

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["LineUserId"] = lineProfile.userId;
                TempData["LineUserName"] = lineProfile.displayName;
                TempData["LineUserPictureUrl"] = lineProfile.pictureUrl;

                ModelState.AddModelError("","註冊失敗," + ex.Message); 
                return View(vm);
            }
            
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
                ModelState.AddModelError("", "新增失敗，" + ex.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }
    }

}