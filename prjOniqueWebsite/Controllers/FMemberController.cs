using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    [Authorize (Roles = "Member")]
    public class FMemberController : Controller
    {
        private readonly OniqueContext _context;
        private readonly UserInfoService _userInfoService;
        OrderDao dao = null;

        public FMemberController(OniqueContext context,UserInfoService userInfoService)
        {
            _context = context;
            _userInfoService = userInfoService;
            dao = new OrderDao(_context);

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
            var mem = (from m in _context.Members
                       join c in _context.Citys
                       on m.Citys equals c.CityId
                       join a in _context.Areas
                       on m.Areas equals a.AreaId
                       where m.MemberId == member.MemberId
                       select new FMemberDto{
                           MemberId = member.MemberId,
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
            var mem = (from m in _context.Members
                          join c in _context.Citys
                          on m.Citys equals c.CityId
                          join a in _context.Areas
                          on m.Areas equals a.AreaId
                          where m.MemberId == id
                          select new FMemberDto
                          {
                              MemberId = id,
                              Name = m.Name,
                              DateOfBirth = Convert.ToDateTime(m.DateOfBirth).ToString("yyyy-MM-dd"),
                              Email = m.Email,
                              Phone = m.Phone,
                              Gender = m.Gender ? "女" : "男",
                              Citys = c.CityName,
                              Areas = a.AreaName,
                              Address = m.Address
                          }).FirstOrDefault();
            return PartialView(mem);
        }
        [HttpPost]
        public IActionResult MemberInfoEdit(FMemberDto fMemberDto)
        {
            var member = _context.Members.FirstOrDefault(m => m.MemberId == fMemberDto.MemberId);

                member.Name = fMemberDto.Name;
                member.Phone = fMemberDto.Phone;
                member.Citys = Convert.ToInt32(fMemberDto.Citys);
                member.Areas = Convert.ToInt32(fMemberDto.Areas);
                member.Address = fMemberDto.Address;
            _context.SaveChanges();
            return PartialView("MemberInfo");
        }
        
        public IActionResult MemberOrder()
        {
            Members member = _userInfoService.GetMemberInfo();
            
            return PartialView(member);
        }
        
        public IActionResult MemberMyKeep()
        {
            Members member = _userInfoService.GetMemberInfo();
            return PartialView();
        }
        
        public IActionResult MemberPassword()
        {
            Members member = _userInfoService.GetMemberInfo();

            return PartialView();
        }
    }
}
