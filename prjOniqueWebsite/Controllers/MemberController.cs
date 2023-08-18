﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;

namespace prjOniqueWebsite.Controllers
{
    public class MemberController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao;
        MemberService _service;
        public MemberController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
            _service = new MemberService(_context, _environment);
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
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                _service.MemberCreate(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "新增失敗，" + ex.Message);
                return View(vm);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var dto = _dao.GetMemberById(id);

            MemberEditVM vm = new MemberEditVM
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
        public IActionResult Edit(MemberEditVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                _service.MemberEdit(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "新增失敗，" + ex.Message);
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var member = _context.Members.FirstOrDefault(m => m.MemberId == id);
                if (member != null)
                {
                    _context.Members.Remove(member);
                }
                _context.SaveChanges();
                var result = new ApiResult { StatusCode = 200, StatusMessage = "刪除資料成功!" };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message };
                return Json(result);

            }
           
            return RedirectToAction("Index");
        }

    }
}
