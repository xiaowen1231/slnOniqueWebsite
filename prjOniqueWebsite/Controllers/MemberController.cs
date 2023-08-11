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

                //member.Name = vm.Name;
                //member.Password = vm.Password;
                //member.Email = vm.Email;
                //member.Phone = vm.Phone;
                //member.Gender = Convert.ToBoolean(vm.Gender);
                //member.Citys = Convert.ToInt32(vm.Citys);
                //member.Areas = Convert.ToInt32(vm.Areas);
                //member.Address = vm.Address;
                //member.MemberLevel = Convert.ToInt32(vm.MemberLevel);
                //member.RegisterDate = DateTime.Now;
                //member.DateOfBirth = Convert.ToDateTime(vm.DateOfBirth);
                
                //_context.Members.Add(member); 
                //_context.SaveChanges();
                //if (vm.Photo != null)
                //{
                //    string fileName = $"MemberId_{member.MemberId}.jpg";
                //    member.PhotoPath = fileName;
                //    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);
                //    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                //    {
                //        vm.Photo.CopyTo(fileStream);
                //    }   
                //}
                //else
                //{
                //    // 如果沒有上傳新照片，使用預設的照片路徑和檔名
                //    string defaultFileName = $"MemberId_{member.MemberId}.jpg";
                //    member.PhotoPath = defaultFileName;
                //    // 取得預設照片的完整路徑
                //    string defaultPhotoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", defaultFileName);
                //    // 複製預設照片到指定路徑
                //    string defaultPhotoSourcePath = Path.Combine(_environment.WebRootPath, "images", "uploads", "members", "default.jpg");
                //    System.IO.File.Copy(defaultPhotoSourcePath, defaultPhotoPath, true);
                //}
                //_context.Update(member);
                //_context.SaveChanges();
                _dao.CreateMember(vm);
                return RedirectToAction("Index");
            }
           return View(vm);
        }

        public IActionResult Edit(int id)
        {
            _dao.GetMemberById(id);
            //MemberEditDto dto = (from m in _context.Members
            //                      join c in _context.Citys
            //                      on m.Citys equals c.CityId
            //                      join a in _context.Areas
            //                      on m.Areas equals a.AreaId
            //                      join ml in _context.MemberLevel
            //                      on m.MemberLevel equals ml.MemberLevelId
            //                      where m.MemberId == id
            //                      select new MemberEditDto
            //                      {
            //                          MemberId = m.MemberId,
            //                          PhotoPath = m.PhotoPath,
            //                          Name = m.Name,
            //                          Password = m.Password,
            //                          Email = m.Email,
            //                          Phone = m.Phone,
            //                          Gender = m.Gender ? "女" : "男",
            //                          Citys = c.CityName,
            //                          Areas = a.AreaName,
            //                          Address = m.Address,
            //                          MemberLevel = ml.MemberLevelName,
            //                          RegisterDate =Convert.ToDateTime( m.RegisterDate).ToString("yyyy-MM-dd"),
            //                          DateOfBirth = Convert.ToDateTime( m.DateOfBirth).ToString("yyyy-MM-dd")
            //                      }).FirstOrDefault();
            //MemberVM vm = new MemberVM
            //{
            //    MemberId = dto.MemberId, 
            //    PhotoPath = dto.PhotoPath,
            //    Name= dto.Name,
            //    Password = dto.Password,
            //    Email = dto.Email,
            //    Phone = dto.Phone,
            //    Gender = dto.Gender,
            //    Citys = dto.Citys,
            //    Areas = dto.Areas,
            //    Address = dto.Address,
            //    MemberLevel = dto.MemberLevel,
            //    RegisterDate = dto.RegisterDate,
            //    DateOfBirth = dto.DateOfBirth
            //};
            
            return View(/*vm*/);
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
