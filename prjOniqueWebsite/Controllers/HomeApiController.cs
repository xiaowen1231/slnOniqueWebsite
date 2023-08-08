using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class HomeApiController : Controller
    {
        public IActionResult IsLogin()
        {
            bool isLogin = HttpContext.Session.Keys.Contains("Login") ? true : false;
            return Json(isLogin);
        }
        public IActionResult IsLoginByEmployee()
        {
            bool isLogin = HttpContext.Session.Keys.Contains("EmployeeLogin") ? true : false;
            return Json(isLogin);
        }
        public IActionResult UpdataNav()
        {
            string json = HttpContext.Session.GetString("Login");
            
            Members member = JsonSerializer.Deserialize<Members>(json);

            return Json(member);
        }

        public IActionResult UpdataNavBackground()
        {
            string json = HttpContext.Session.GetString("EmployeeLogin");

            Employees employees = JsonSerializer.Deserialize<Employees>(json);

            return Json(employees);
        }


    }
}
