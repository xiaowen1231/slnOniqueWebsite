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

        //public async IActionResult Create()
        //{
            
        //    return View();
        //}



        public IActionResult Edit()
        {
            return View();
        }
    }
}
