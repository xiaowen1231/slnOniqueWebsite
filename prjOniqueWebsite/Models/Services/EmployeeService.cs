using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Services
{
    public class EmployeeService
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        EmployeeDao dao;

        public EmployeeService(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            dao = new EmployeeDao(_context, _environment);
        }

        public void EmployeeCreateCheck(EmployeeVM vm)
        {
            var isEmailUsed = dao.GetEmployeeByEmail(vm.Email);
            var isPhoneUsed = dao.GetEmployeeByPhone(vm.Phone);
            if (isEmailUsed != null)
            {
                throw new Exception("已有此信箱!");
            }
            if (isPhoneUsed != null)
            {
                throw new Exception("已有此電話號碼!");
            }
            if (Convert.ToDateTime(vm.DateOfBirth) >= DateTime.Now)
            {
                throw new Exception("輸入的生日有誤");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            dao.CreateEmployee(vm);
        }

        public void EmployeeEditCheck(EmployeeEditVM vm)
        {
            var isEmailUsed = dao.GetEmployeeByEmail(vm.Email);
            if (isEmailUsed != null && isEmailUsed.EmployeeId != vm.EmployeeId)
            {
                throw new Exception("此信箱已被其他員工使用，無法修改!");
            }
            var isPhoneUsed = dao.GetEmployeeByPhone(vm.Phone);
            if (isPhoneUsed != null && isPhoneUsed.EmployeeId != vm.EmployeeId)
            {
                throw new Exception("已有此電話號碼!");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            dao.EditEmployee(vm);
        }
    }
}
