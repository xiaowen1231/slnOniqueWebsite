using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System;

namespace prjOniqueWebsite.Models.Daos
{
    public class EmployeeDao
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;

        public EmployeeDao(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public EmployeeEditDto GetEmployeeById(int id)
        {
            EmployeeEditDto employee = (from e in _context.Employees
                                   join el in _context.EmployeeLevel
                                   on e.Position equals el.EmployeeLevelId
                                   join c in _context.Citys
                                   on e.Citys equals c.CityId
                                   join a in _context.Areas
                                   on e.Areas equals a.AreaId
                                   where e.EmployeeId == id
                                   select new EmployeeEditDto
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

        public void EditEmployee(EmployeeEditVM vm)
        {
            var employee = _context.Employees.Where(e => e.EmployeeId == vm.EmployeeId).FirstOrDefault();
            if (vm.Photo != null)
            {
                string fileName = $"EmployeeId_{employee.EmployeeId}.jpg";
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/Employee", fileName);

                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                        vm.Photo.CopyTo(fileStream);

                }
                employee.PhotoPath = fileName;
            }
            employee.EmployeeName = vm.EmployeeName;
            employee.Password = vm.Password;
            employee.Phone = vm.Phone;
            employee.Citys = Convert.ToInt32(vm.Citys);
            employee.Areas = Convert.ToInt32(vm.Areas);
            employee.Address = vm.Address;
            employee.Position = Convert.ToInt32(vm.EmployeeLevel);
            

            _context.SaveChanges();
        }

        public void CreateEmployee(EmployeeVM vm) 
        {
            var employee = new Employees();
            employee.EmployeeId = vm.EmployeeId;
            employee.EmployeeName = vm.EmployeeName;
            employee.DateOfBirth = Convert.ToDateTime(vm.DateOfBirth);
            employee.Gender = vm.Gender == "男" ? true : false;
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
            if (vm.Photo != null)
            {
                string fileName = $"EmployeeId_{employee.EmployeeId}.jpg";
                employee.PhotoPath = fileName;
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/employee", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }

                _context.Update(employee);
                _context.SaveChanges();
            }

        }

        public Employees GetEmployeeByEmail(string email)
        {               
            var employee = _context.Employees.FirstOrDefault(e => e.Email == email);
            if (employee == null)
            {
                return null;
            }
            return employee;
        }
        public Employees GetEmployeeByPhone(string phone)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Phone == phone);
            if (employee == null)
            {
                return null;
            }
            return employee;
        }

        public void DeleteEmployee(Employees employee) 
        {
            _context.Remove(employee);
            _context.SaveChanges();
        }
    }
}
