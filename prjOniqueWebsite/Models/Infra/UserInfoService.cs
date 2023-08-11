using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.Infra
{
    public class UserInfoService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserInfoService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public Members GetMemberInfo()
        {
            var claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var memberId = claim.Where(c => c.Type == "MemberId").FirstOrDefault();

            if (memberId == null)
            {
                return null;
            }

            int id = Convert.ToInt32(memberId.Value);

            Members member = new OniqueContext().Members.FirstOrDefault(m => m.MemberId == id);

            return member;
        }

        public Employees GetEmployeeInfo()
        {
            var claim = _contextAccessor.HttpContext.User.Claims.ToList();

            var employeeId = claim.Where(c => c.Type == "EmployeeId").FirstOrDefault();

            if (employeeId == null)
            {
                return null;
            }

            int id = Convert.ToInt32(employeeId.Value);

            Employees employee = new OniqueContext().Employees.FirstOrDefault(e => e.EmployeeId == id);

            return employee;
        }
    }
}
