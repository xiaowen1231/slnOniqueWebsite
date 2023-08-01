using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly OniqueContext _context;
        public EmployeesController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
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



        public IActionResult Edit()
        {
            return View();
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
