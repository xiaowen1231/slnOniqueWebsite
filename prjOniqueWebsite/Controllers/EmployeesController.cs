using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;

namespace prjOniqueWebsite.Controllers
{
    [Authorize(Roles = "經理")]
    public class EmployeesController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        EmployeeDao dao=null;
        public EmployeesController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            dao = new EmployeeDao(_context, _environment);
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

                if(vm.Photo.Length != 10) 
                {
                    ModelState.AddModelError("Email", "電話號碼為10位數字!");
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

                if (Convert.ToDateTime(vm.DateOfBirth) >= DateTime.Today)
                {
                    ModelState.AddModelError("DateOfBirth", "日期輸入錯誤");
                    return View(vm);
                }
                if (Convert.ToDateTime(vm.DateOfBirth) >= SqlDateTime.MaxValue.Value || Convert.ToDateTime(vm.DateOfBirth) < SqlDateTime.MinValue.Value)
                {
                    ModelState.AddModelError("DateOfBirth", "日期輸入錯誤");
                    return View(vm);
                }

                dao.CreateEmployee(vm);
                //Employees employee = new Employees();
                //dao.CreatePhoto(employee, vm);
            
                return RedirectToAction("Index");
            }
            return View(vm);



        }



        public IActionResult Edit(int id)
        {
            EmployeeVM employee = dao.GetEmployeeById(id); 
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeVM vm)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == vm.EmployeeId);

            //var employee = dao.GetEmployeeById(vm.EmployeeId);

            //if (employee != null)
            //{
            //    if (vm.Photo != null)
            //    {
            //        string fileName = $"EmployeeId_{employee.EmployeeId}.jpg";
            //        string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/Employee", fileName);

            //        using (var fileStream = new FileStream(photoPath, FileMode.Create))
            //        {
            //            vm.Photo.CopyTo(fileStream);

            //        }
            //        employee.PhotoPath = fileName;
            //    }
            //    employee.EmployeeName = vm.EmployeeName;
            //    employee.Password = vm.Password;
            //    employee.Phone = vm.Phone;
            //    employee.Citys = Convert.ToInt32(vm.Citys);
            //    employee.Areas = Convert.ToInt32(vm.Areas);
            //    employee.Address = vm.Address;
            //    employee.Position = Convert.ToInt32(vm.EmployeeLevel);
            //}

            //_context.SaveChanges();
            dao.EditEmployee(employee, vm);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(EmployeeVM vm)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == vm.EmployeeId);

            if (employee != null) 
            {
                //_context.Remove(employee);
                //_context.SaveChanges();
                dao.DeleteEmployee(employee);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
