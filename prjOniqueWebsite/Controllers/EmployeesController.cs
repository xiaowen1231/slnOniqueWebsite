using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
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
            //IEnumerable<Employees> datas = null;

            //if (string.IsNullOrEmpty(vm.txtKeyword))
            //    datas = from e in _context.Employees
            //            select e;
            //else
            //{
            //    datas = _context.Employees.Where(e => e.EmployeeName.Contains(vm.txtKeyword) ||
            //          e.Phone.Contains(vm.txtKeyword) ||
            //          e.Email.Contains(vm.txtKeyword));
            //}
            return View();
        }

        public IActionResult Create()
        {
            //EmployeeVM employee = (from e in _context.Employees
            //                             join el in _context.EmployeeLevel
            //                             on e.Position equals el.EmployeeLevelId
            //                             join c in _context.Citys
            //                             on e.Citys equals c.CityId
            //                             join a in _context.Areas
            //                             on e.Areas equals a.AreaId
            //                             select new EmployeeVM
            //                             {
            //                                 EmployeeLevel = el.EmployeeLevelName,
            //                                 Citys = c.CityName,
            //                                 Areas = a.AreaName,
            //                             }).FirstOrDefault();
            //ViewBag.employeeLevel = employee.EmployeeLevel;


            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeVM vm)
        {
            if (ModelState.IsValid )
            {
                Employees employee = new Employees();
                employee.PhotoPath = vm.PhotoPath;
                employee.EmployeeId = vm.EmployeeId;
                employee.EmployeeName = vm.EmployeeName;
                employee.DateOfBirth = Convert.ToDateTime(vm.DateOfBirth);
                employee.Gender = vm.Gender;
                employee.Position = Convert.ToInt32(vm.EmployeeLevel);
                employee.Phone = vm.Phone;
                employee.Email = vm.Email;
                employee.Password = vm.Password;
                employee.RegisterDate = Convert.ToDateTime(vm.RegisterDate);
                employee.Citys = Convert.ToInt32(vm.Citys);
                employee.Areas = Convert.ToInt32(vm.Areas);
                employee.Address = vm.Address;
                _context.Add(employee);
                _context.SaveChanges();
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
                                       Gender = e.Gender ,
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
