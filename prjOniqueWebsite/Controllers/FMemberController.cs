using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class FMemberController : Controller
    {
        private readonly OniqueContext _context;
        public FMemberController(OniqueContext context)
        {
            _context = context;
        }

        [TypeFilter(typeof(MemberVerify))]
        public IActionResult Index( string display)
        {
            ViewBag.Display = display;
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            var photo = (from m in _context.Members
                         where m.MemberId == member.MemberId
                         select new FMemberDto{
                             MemberId = member.MemberId,
                             PhotoPath = m.PhotoPath
                         }).FirstOrDefault();
            return View(photo);
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult MemberInfo( )
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
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
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult MemberOrder()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            return PartialView();
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult MemberMyKeep()
        {
            return PartialView();
        }
        [TypeFilter(typeof(MemberVerify))]
        public IActionResult MemberPassword()
        {
            return PartialView();
        }
    }
}
