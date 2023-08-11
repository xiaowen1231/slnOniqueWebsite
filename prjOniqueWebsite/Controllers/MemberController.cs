using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;

namespace prjOniqueWebsite.Controllers
{
    public class MemberController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao = null;
        public MemberController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MemberVM vm)
        {
            if (ModelState.IsValid)
            {
                bool isPhoneUsed = _context.Members.Any(e => e.Phone == vm.Phone);
                bool isEmailUsed = _context.Members.Any(e => e.Email == vm.Email);
                
                if (isPhoneUsed)
                {
                    ModelState.AddModelError("Phone", "手機號碼已被使用");
                    return View(vm);
                }

                if (isEmailUsed)
                {
                    ModelState.AddModelError("Email", "信箱已被使用");
                    return View(vm);
                }
                //todo日期判斷
                if (Convert.ToDateTime(vm.DateOfBirth) >= DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "日期輸入錯誤");
                    return View(vm);
                }
                _dao.CreateMember(vm);
                return RedirectToAction("Index");
            }
           return View(vm);
        }

        public IActionResult Edit(int id)
        {
            var dto = _dao.GetMemberById(id);
           
            MemberVM vm = new MemberVM
            {
                MemberId = dto.MemberId,
                PhotoPath = dto.PhotoPath,
                Name = dto.Name,
                Password = dto.Password,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                Citys = dto.Citys,
                Areas = dto.Areas,
                Address = dto.Address,
                MemberLevel = dto.MemberLevel,
                RegisterDate = dto.RegisterDate,
                DateOfBirth = dto.DateOfBirth
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(MemberVM vm)
        {
            var member = _context.Members.FirstOrDefault(m => m.MemberId == vm.MemberId);

            _dao.EditMember(member, vm);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var member = _context.Members.FirstOrDefault(m=>m.MemberId == id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
