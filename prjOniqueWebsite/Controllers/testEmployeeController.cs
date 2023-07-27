using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Controllers
{
    public class testEmployeeController : Controller
    {
        private readonly OniqueContext _context;
        public testEmployeeController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            EmployeeCreateVM employee = (from e in _context.Employees
                                        join el in _context.EmployeeLevel
                                        on e.Position equals el.EmployeeLevelId
                                        join c in _context.Citys
                                        on e.Citys equals c.CityId
                                        join a in _context.Areas
                                        on e.Areas equals a.AreaId
                                        select new EmployeeCreateVM
                                        {
                                            
                                            //PhotoPath = e.PhotoPath,
                                            
                                            
                                            EmployeeLevel = el.EmployeeLevelName,
                                            
                                            Citys = c.CityName,
                                            Areas = a.AreaName,
                                           

                                        }).FirstOrDefault();

            return View(employee);
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateVM vm)
        {
            //Employees employee = new Employees();
            //employee.EmployeeName = vm.EmployeeName;
            //employee.DateOfBirth = vm.DateOfBirth;
            //employee.Position = vm.Position;
            //employee.Phone = vm.Phone;
            //employee.Email = vm.Email;
            //employee.Password = vm.Password;
            //employee.Position = vm.Position ;
            //employee.RegisterDate = vm.RegisterDate;
            //employee.Citys = vm.Citys;
            //employee.Areas = vm.Areas;
            //employee.Address = vm.Address;
            //_context.Add(db);
            //_context.SaveChanges();

            //return RedirectToAction("Index");
            return null;
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
