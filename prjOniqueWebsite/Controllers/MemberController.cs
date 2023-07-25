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
        public IActionResult Edit(int id =5)
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
            return View(member);
        }
        [HttpPost]
        public IActionResult Edit(MemberVM vm)
        {
            
           string name = vm.Name;
            string password = vm.Password;
            string phone = vm.Phone;
            string email = vm.Email;
            string gender = vm.Gender;
            string citys = vm.Citys;
            string areas = vm.Areas;
            string address = vm.Address;
            string memberlevel = vm.MemberLevel;

            return View(vm);
        }
    }
}
