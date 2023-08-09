﻿using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;

namespace prjOniqueWebsite.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        public EmployeesController(OniqueContext context, IWebHostEnvironment environment)
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
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeVM vm)
        {
            if (ModelState.IsValid )
            {
                Employees employee = new Employees();
                bool isPhoneUsed = _context.Employees.Any(e => e.Phone == vm.Phone);
                bool isEmailUsed = _context.Employees.Any(e => e.Email == vm.Email);

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

                if (vm.Gender != "男" && vm.Gender != "女")
                {
                    ModelState.AddModelError("Gender", "請選擇性別");
                    return View(vm);
                }

                if (!int.TryParse(vm.Citys, out int parsedCitys))
                {
                    ModelState.AddModelError("Citys", "請選擇居住城市");
                    return View(vm);
                }
                employee.Citys = parsedCitys;

                DateTime parsedDateOfBirth;
                if (!string.IsNullOrWhiteSpace(vm.DateOfBirth) && DateTime.TryParse(vm.DateOfBirth, out parsedDateOfBirth))
                {
                    if (parsedDateOfBirth > DateTime.Today)
                    {
                        ModelState.AddModelError("DateOfBirth", "出生日期時間錯誤");
                        return View(vm);
                    }

                    //SqlDateTime 引起的錯誤，值需要在 1753 年到 9999 年之間
                    if (parsedDateOfBirth < SqlDateTime.MinValue.Value || parsedDateOfBirth > SqlDateTime.MaxValue.Value)
                    {
                        ModelState.AddModelError("DateOfBirth", "請選擇生日");
                        return View(vm);
                    }
                    employee.DateOfBirth = parsedDateOfBirth;
                }
                else
                {
                    ModelState.AddModelError("DateOfBirth", "請選擇生日");
                    return View(vm);
                }
                employee.EmployeeId = vm.EmployeeId;
                employee.EmployeeName = vm.EmployeeName;
                //employee.DateOfBirth = Convert.ToDateTime(vm.DateOfBirth);
                employee.Gender = vm.Gender=="男"? true : false;
                employee.Position = Convert.ToInt32(vm.EmployeeLevel);
                employee.Phone = vm.Phone;
                employee.Email = vm.Email;
                employee.Password = vm.Password;
                employee.RegisterDate = Convert.ToDateTime(vm.RegisterDate);
                employee.Areas = Convert.ToInt32(vm.Areas);
                employee.Address = vm.Address;
                _context.Add(employee);
                _context.SaveChanges();

                if (vm.Photo != null)
                {
                    string fileName = $"EmployeeId_{employee.EmployeeId}.jpg";
                    employee.PhotoPath = fileName;
                    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/employee", fileName);
                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        vm.Photo.CopyTo(fileStream);
                    }

                    _context.Update(employee);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(vm);



        }



        public IActionResult Edit(int id)
        {
            EmployeeVM employee =  (from e in _context.Employees
                                   join el in _context.EmployeeLevel 
                                   on e.Position equals el.EmployeeLevelId
                                   join c in _context.Citys 
                                   on e.Citys equals c.CityId
                                   join a in _context.Areas 
                                   on e.Areas equals a.AreaId
                                   where e.EmployeeId == id
                                   select new EmployeeVM 
                                   {
                                       EmployeeId = e.EmployeeId,
                                       EmployeeName = e.EmployeeName,
                                       PhotoPath = e.PhotoPath,
                                       DateOfBirth = Convert.ToDateTime(e.DateOfBirth).ToString("yyyy-MM-dd"),
                                       Gender = e.Gender==true? "男" : "女",
                                       Phone = e.Phone,
                                       Email = e.Email,
                                       Password = e.Password,
                                       EmployeeLevel = el.EmployeeLevelName,
                                       RegisterDate = Convert.ToDateTime(e.RegisterDate).ToString("yyyy-MM-dd"),
                                       Citys = c.CityName,
                                       Areas = a.AreaName,
                                       Address = e.Address,
                                   }).FirstOrDefault();
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeVM vm)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == vm.EmployeeId);

            if (employee != null) 
            {
                if (vm.Photo != null)
                {
                    string fileName = $"EmployeeId_{employee.EmployeeId}.jpg";
                    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/Employee", fileName);
                    
                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        vm.Photo.CopyTo(fileStream);
                        
                    }
                    employee.PhotoPath = fileName;
                }

                employee.EmployeeName = vm.EmployeeName;
                employee.Password = vm.Password;
                employee.Phone = vm.Phone;
                employee.Citys = Convert.ToInt32(vm.Citys);
                employee.Areas = Convert.ToInt32(vm.Areas);
                employee.Address = vm.Address;
                employee.Position = Convert.ToInt32(vm.EmployeeLevel);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(EmployeeVM vm)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == vm.EmployeeId);

            if (employee != null) 
            {
                _context.Remove(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
    }
}
