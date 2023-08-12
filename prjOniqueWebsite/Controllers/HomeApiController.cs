using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class HomeApiController : Controller
    {
        private readonly UserInfoService _userInfoService;
        private readonly OniqueContext _context;
        public HomeApiController(UserInfoService userInfoService,OniqueContext context)
        {
            _userInfoService = userInfoService;
            _context = context;
        }
        public IActionResult IsLogin()
        {
            bool isLogin = HttpContext.User.Identity.IsAuthenticated ? true : false;
            return Json(isLogin);
        }
        public IActionResult IsLoginByEmployee()
        {
            bool isLogin = HttpContext.Session.Keys.Contains("EmployeeLogin") ? true : false;
            return Json(isLogin);
        }
        public IActionResult UpdataNav()
        {
            if (HttpContext.User.IsInRole("Member"))
            {
                Members member = _userInfoService.GetMemberInfo();
                var dto = new
                {
                    Role = "Member",
                    datas = member
                };
                return Json(dto);

            }
            else
            {
                Employees employee = _userInfoService.GetEmployeeInfo();
                var dto = new
                {
                    Role = "Employee",
                    datas = employee
                };
                return Json(dto);
            }

        }

        public IActionResult GetCarouselInfo()
        {
            var data = _context.Discounts.Select(d => new
            {
                photoPath = d.PhotoPath,
                discountId = d.Id
            });

            return Json(data.ToList());
        }
    }
}
