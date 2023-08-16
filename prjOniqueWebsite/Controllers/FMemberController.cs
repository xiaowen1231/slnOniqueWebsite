using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;
using System.Drawing.Printing;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    [Authorize (Roles = "Member")]
    public class FMemberController : Controller
    {
        private readonly OniqueContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IWebHostEnvironment _environment;
        MemberService _service;
        MemberDao _memDao;
        OrderDao _orderDao = null;

        public FMemberController(OniqueContext context,UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
            _service = new MemberService(_context,_environment);
            _memDao = new MemberDao(_context,_environment);
            _orderDao = new OrderDao(_context);
            
        }


        public IActionResult Index( string display)
        {
            ViewBag.Display = display;
            Members member = _userInfoService.GetMemberInfo();
            var photo = (from m in _context.Members
                         where m.MemberId == member.MemberId
                         select new FMemberDto{
                             MemberId = member.MemberId,
                             PhotoPath = m.PhotoPath
                         }).FirstOrDefault();
            return View(photo);
        }

        public IActionResult MemberInfo( )
        {
            Members member = _userInfoService.GetMemberInfo();
            FMemberDto mem = (from m in _context.Members
                       join c in _context.Citys
                       on m.Citys equals c.CityId
                       join a in _context.Areas
                       on m.Areas equals a.AreaId
                       where m.MemberId == member.MemberId
                       select new FMemberDto{
                           MemberId = m.MemberId,
                           Name = m.Name,
                           DateOfBirth = Convert.ToDateTime(m.DateOfBirth).ToString("yyyy-MM-dd"),
                           Email = m.Email,
                           Phone = m.Phone,
                           Gender = m.Gender? "女":"男",
                           Citys = c.CityName,
                           Areas = a.AreaName,
                           Address = m.Address
            }).FirstOrDefault();
            return PartialView(mem);
        }
        public IActionResult MemberInfoEdit(int id)
        {
            var dto = _memDao.GetFMemberById(id);
            FMemberEditVM vm = new FMemberEditVM()
            {
                MemberId = dto.MemberId,
                Name = dto.Name,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                Citys = dto.Citys,
                Areas = dto.Areas,
                Address = dto.Address
            };

            return PartialView(vm);
        }
        [HttpPost]
        public IActionResult MemberInfoEdit(FMemberEditVM vm)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                result = new ApiResult { StatusCode = 500, StatusMessage = errors.FirstOrDefault() };
                return Json(result);
            }
            var member = _context.Members.FirstOrDefault(m => m.MemberId == vm.MemberId);
            try
            {
                _service.FMemberEdit(vm);
                result = new ApiResult { StatusCode = 200, StatusMessage = "編輯資料成功!" };
                return Json(result);
            }catch (Exception ex)
            {
                result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message};
                return Json(result);

            }

        }
        
        public IActionResult MemberOrder()
        {
            Members member = _userInfoService.GetMemberInfo();
           
            return PartialView(member);
        }
        
        public IActionResult MemberMyKeep()
        {
            Members member = _userInfoService.GetMemberInfo();
            return PartialView(member);
        }
        
        public IActionResult MemberPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult MemberPassword(FMemberPasswordVM vm)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                result = new ApiResult { StatusCode = 500, StatusMessage = errors.FirstOrDefault() };
                return Json(result);
            }
            try
            {
                int loginMemId = _userInfoService.GetMemberInfo().MemberId;
                _service.FMemberPasswordReset(vm, loginMemId);
                result = new ApiResult { StatusCode = 200, StatusMessage = "編輯資料成功!" };
                return Json(result);
            }
            catch (Exception ex)
            {
                result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message };
                return Json(result);

            }
        }
    }
}
