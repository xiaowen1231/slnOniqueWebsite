using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;

namespace prjOniqueWebsite.Controllers
{
    public class MemberController : Controller
    {
        private readonly OniqueContext _context;
        public MemberController(OniqueContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            MemberVM member = (from m in _context.Members
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
            ViewBag.memberlevel = member.MemberLevel;
            ViewBag.city = member.Citys;
            ViewBag.area = member.Areas;
            return View();
        }
        public IActionResult Edit(int id)
        {
            MemberVM member = (from m in _context.Members
                                  join c in _context.Citys
                                  on m.Citys equals c.CityId
                                  join a in _context.Areas
                                  on m.Areas equals a.AreaId
                                  join ml in _context.MemberLevel
                                  on m.MemberLevel equals ml.MemberLevelId
                                  where m.MemberId == id
                                  select new MemberVM
                                  {
                                      PhotoPath = m.PhotoPath,
                                      MemberId = m.MemberId,
                                      Name = m.Name,
                                      Password = m.Password,
                                      Phone = m.Phone,
                                      Email = m.Email,
                                      Gender = m.Gender ? "女" : "男",
                                      Citys = c.CityName,
                                      Areas = a.AreaName,
                                      Address = m.Address,
                                      DateOfBirth = Convert.ToDateTime( m.DateOfBirth).ToString("yyyy-MM-dd"),
                                      RegisterDate =Convert.ToDateTime( m.RegisterDate).ToString("yyyy-MM-dd"),
                                      MemberLevel = ml.MemberLevelName
                                  }).FirstOrDefault();
            
           
            return View(member);
        }
        [HttpPost]
        public IActionResult Edit(MemberVM member ,int id)
        {
            var level = new MemberLevel();
            var city = new Citys();
            var area = new Areas();
            var mem = _context.Members.Where(Members => Members.MemberId == id).FirstOrDefault();

            mem.PhotoPath = $"MemberId_{id}.jpg";
            mem.Name = member.Name;
            mem.Password = member.Password;
            if (mem.Email == member.Email && mem.Phone == member.Phone)
            {

            }
            else
            {
                mem.Phone = member.Phone;
                mem.Email = member.Email;
            }
            mem.DateOfBirth = Convert.ToDateTime(member.DateOfBirth);
            mem.RegisterDate = Convert.ToDateTime(member.RegisterDate);
            mem.MemberLevel = _context.MemberLevel.Where(c => c.MemberLevelName == level.MemberLevelName).Select(c => c.MemberLevelId).FirstOrDefault();
            mem.Citys = _context.Citys.Where(c => c.CityName == city.CityName).Select(c => c.CityId).FirstOrDefault();
            mem.Areas = _context.Areas.Where(c => c.AreaName == area.AreaName).Select(c => c.AreaId).FirstOrDefault();
            mem.Address = member.Address;
            _context.Members.Add(mem);
            _context.SaveChanges();
            return View(mem);
        }
    }
}
