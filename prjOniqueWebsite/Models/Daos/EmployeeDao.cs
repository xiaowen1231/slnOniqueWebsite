using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Daos
{
    public class EmployeeDao
    {
        private readonly OniqueContext _context; 

        public EmployeeDao(OniqueContext context)
        {
            _context = context;
        }

        public EmployeeVM GetEmployeeById(int id)
        {
            EmployeeVM employee = (from e in _context.Employees
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
                                       Gender = e.Gender == true ? "男" : "女",
                                       Phone = e.Phone,
                                       Email = e.Email,
                                       Password = e.Password,
                                       EmployeeLevel = el.EmployeeLevelName,
                                       RegisterDate = Convert.ToDateTime(e.RegisterDate).ToString("yyyy-MM-dd"),
                                       Citys = c.CityName,
                                       Areas = a.AreaName,
                                       Address = e.Address,
                                   }).FirstOrDefault();
            return employee;
        }

        public void UpdateEmployee(EmployeeVM employee)
        {
            var Employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

            if (Employee != null)
            {
                Employee.EmployeeName = employee.EmployeeName;
                Employee.Password = employee.Password;
                Employee.Phone = employee.Phone;
                Employee.Citys = Convert.ToInt32(employee.Citys);
                Employee.Areas = Convert.ToInt32(employee.Areas);
                Employee.Address = employee.Address;
                Employee.Position = Convert.ToInt32(employee.EmployeeLevel);

                _context.SaveChanges();
            }
        }

        public void DeleteEmployee(EmployeeVM employee)
        {
            var Employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

            if (Employee != null)
            {
                _context.Remove(Employee);
                _context.SaveChanges();
            }
        }


    }
}
