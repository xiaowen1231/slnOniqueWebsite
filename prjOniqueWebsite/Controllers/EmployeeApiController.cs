using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class EmployeeApiController : Controller
    {
        private readonly OniqueContext _context;
        public EmployeeApiController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowEmployeeList()
        {
            var query = from e in _context.Employees
                        join el in _context.EmployeeLevel
                        on e.Position equals el.EmployeeLevelId
                        select new EmployeeListDto 
                        {
                            EmployeeId = e.EmployeeId,
                            PhotoPath = e.PhotoPath,
                            EmployeeName = e.EmployeeName,
                            DateOfBirth = (DateTime)e.DateOfBirth,
                            Gender = e.Gender ? "男" : "女",
                            Phone = e.Phone,
                            Email = e.Email,
                            EmployeeLevelName = el.EmployeeLevelName
                        };

            List<EmployeeListDto> dto = query.ToList();
            return Json(dto);
        }

        public IActionResult LoadCity()
        {
            var citys = from c in _context.Citys
                        select c;
            return Json(citys);
        }
        public IActionResult LoadArea(int cityId)
        {
            var areas = from a in _context.Areas

                        where a.CityId == cityId
                        select a;
            return Json(areas);
        }

        public IActionResult LoadEmployeeLevel() 
        {
            var employeelevel = from e in _context.EmployeeLevel
                                select e;
            return Json(employeelevel);
        }
    }
}
