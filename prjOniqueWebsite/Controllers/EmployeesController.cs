using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using prjOniqueWebsite.Models.Services;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Net;

namespace prjOniqueWebsite.Controllers
{
    [Authorize(Roles = "經理")]
    public class EmployeesController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        EmployeeDao dao=null;
        EmployeeService service = null;
        public EmployeesController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            dao = new EmployeeDao(_context, _environment);
            service = new EmployeeService(_context, _environment);
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
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                service.EmployeeCreateCheck(vm);
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
            var dto = dao.GetEmployeeById(id);
            EmployeeEditVM vm = new EmployeeEditVM
            {
                EmployeeId = dto.EmployeeId,
                EmployeeName = dto.EmployeeName,
                PhotoPath = dto.PhotoPath,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Email = dto.Email,
                Password = dto.Password,
                EmployeeLevel = dto.EmployeeLevel,
                RegisterDate = dto.RegisterDate,
                Citys = dto.Citys,
                Areas = dto.Areas,
                Address = dto.Address,
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                service.EmployeeEditCheck(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "新增失敗，" + ex.Message);
                return View(vm);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(EmployeeVM vm)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == vm.EmployeeId);

            if (employee != null) 
            {
                dao.DeleteEmployee(employee);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
