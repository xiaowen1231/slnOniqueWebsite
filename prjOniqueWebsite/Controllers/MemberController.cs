﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public MemberController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            MemberVM vm = (from m in _context.Members
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
            ViewBag.memberlevel = vm.MemberLevel;
            ViewBag.city = vm.Citys;
            ViewBag.area = vm.Areas;
            return View();
        }
        [HttpPost]
        public IActionResult Create(MemberVM vm)
        {
            var mem = new Members()
            {
                //string fileName = $"MemberId_{member.MemberId}.jpg";
                Name = vm.Name,
                Password = vm.Password,
                Email = vm.Email,
                Phone = vm.Phone,
                Gender = Convert.ToBoolean(vm.Gender),
                Citys = Convert.ToInt32(vm.Citys),
                Areas = Convert.ToInt32(vm.Areas),
                Address = vm.Address,
                MemberLevel = Convert.ToInt32(vm.MemberLevel),
                RegisterDate = DateTime.Now,
                DateOfBirth = Convert.ToDateTime(vm.DateOfBirth),
            };
            
           
            _context.Members.Add(mem); 
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            MemberVM vm = (from m in _context.Members
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
                                      PhotoPath = m.PhotoPath,
                                      Name = m.Name,
                                      Password = m.Password,
                                      Email = m.Email,
                                      Phone = m.Phone,
                                      Gender = m.Gender ? "女" : "男",
                                      Citys = c.CityName,
                                      Areas = a.AreaName,
                                      Address = m.Address,
                                      MemberLevel = ml.MemberLevelName,
                                      RegisterDate =Convert.ToDateTime( m.RegisterDate).ToString("yyyy-MM-dd"),
                                      DateOfBirth = Convert.ToDateTime( m.DateOfBirth).ToString("yyyy-MM-dd")
                                  }).FirstOrDefault();
            
            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(MemberVM vm)
        {
            var member = _context.Members.FirstOrDefault(m => m.MemberId == vm.MemberId);

            if (member != null)
            {
                if (vm.Photo != null)
                {
                    string fileName = $"MemberId_{member.MemberId}.jpg";
                    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);
                    using(var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        vm.Photo.CopyTo(fileStream);
                    }
                }
                member.Name = vm.Name;
                member.Password = vm.Password;
                member.Phone = vm.Phone;
                member.Citys = Convert.ToInt32(vm.Citys);
                member.Areas = Convert.ToInt32(vm.Areas);
                member.Address = vm.Address;
                member.MemberLevel = Convert.ToInt32(vm.MemberLevel);

            }
            _context.SaveChanges();
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
