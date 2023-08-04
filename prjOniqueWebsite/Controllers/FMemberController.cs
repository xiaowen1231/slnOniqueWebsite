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
        public IActionResult Index( string display)
        {
            ViewBag.Display = display;
            return View();
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
        public IActionResult MemberOrder()
        {
            return PartialView();
        }
        public IActionResult MemberMyKeep()
        {
            return PartialView();
        }
        public IActionResult MemberPassword()
        {
            return PartialView();
        }
    }
}
