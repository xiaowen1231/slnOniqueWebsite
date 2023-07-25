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
        public IActionResult Edit()
        {
            EditMemberVM member = (from m in _context.Members
                                  join c in _context.Citys
                                  on m.Citys equals c.CityId
                                  join a in _context.Areas
                                  on m.Areas equals a.AreaId
                                  join ml in _context.MemberLevel
                                  on m.MemberLevel equals ml.MemberLevelId
                                  select new EditMemberVM
                                  {
                                      MemberId = m.MemberId,
                                      Name = m.Name,
                                      Password = m.Password,
                                      Phone = m.Phone,
                                      Email = m.Email,
                                      Gender = m.Gender ? "女" : "男",
                                      Citys = c.CityName,
                                      Areas = a.AreaName,
                                      Address = m.Address,
                                      DateOfBirth = m.DateOfBirth,
                                      RegisterDate = m.RegisterDate,
                                      MemberLevel = ml.MemberLevelName
                                  }).FirstOrDefault();
            EditMemberVM vM = new EditMemberVM()
            {
                Name=member.Name,
                Password=member.Password,
                Phone = member.Phone,
                Email = member.Email,
                Gender = member.Gender,
                Citys = member.Citys,
                Areas = member.Areas,
                Address = member.Address,
                DateOfBirth = member.DateOfBirth,
                RegisterDate = member.RegisterDate,
                MemberLevel = member.MemberLevel
            };
                                   
            //EditMember member = (from m in _context.Members
            //                        join a in _context.Areas
            //                        on m.Areas equals a.AreaId
            //                        select new EditMemberDto
            //                        {
            //                            Area = a.AreaName,
            //                            MemberId = m.MemberId,
            //                            Name = m.Name,
            //                            Gender = m.Gender ? "女" : "男"
            //                        }).FirstOrDefault();
            //EditMemberVM vm = new EditMemberVM()
            //{
            //    Name=member.Name
            //};
            return View();
        }
        [HttpPost]
        public IActionResult Edit(EditMemberDto dto)
        {
           string name = dto.Name;
            string password = dto.Password;
            string phone = dto.Phone;
            string email = dto.Email;
            string gender = dto.Gender;
            string citys = dto.Citys;
            string areas = dto.Areas;
            string address = dto.Address;
            string memberlevel = dto.MemberLevel;

            return View();
        }
    }
}
