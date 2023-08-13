using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;
using System.Data;

namespace prjOniqueWebsite.Controllers
{
    [Authorize(Roles = "一般員工,經理")]
    public class DiscountManageController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        private DiscountService _service;

        public DiscountManageController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _service = new DiscountService(_context,_environment);

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
        public IActionResult Create(BgDiscointCreateVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                _service.Create(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "新增優惠失敗!" + ex.Message);
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            var discount = _context.Discounts.Where(d => d.Id == id)
                .Select(d => new BgDiscointCreateVM
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    BeginDate = d.BeginDate,
                    EndDate = d.EndDate,
                    PhotoPath = d.PhotoPath,
                    DiscountMethod = d.DiscountMethod
                }).FirstOrDefault();

            return View(discount);

        }

        [HttpPost]
        public IActionResult Edit(BgDiscointCreateVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                _service.Edit(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "修改失敗! " + ex.Message);
                return View(vm);
            }
            return RedirectToAction("Index");
        }

    }
}
