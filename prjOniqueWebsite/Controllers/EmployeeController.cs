using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly OniqueContext _context;

        public EmployeeController(OniqueContext context)
        {
            _context = context;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeName,DateOfBirth,Gender,Phone,Email," +
            "Password,EmployeeLevelName,RegisterDate,AreasNavigation,CitysNavigation,Address")] EmployeeDto employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }



        public IActionResult Edit()
        {
            return View();
        }
    }
}
