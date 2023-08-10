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
            int id = Convert.ToInt32(_contextAccessor.HttpContext.User.Identity.Name);

            Members member = new OniqueContext().Members.FirstOrDefault(m => m.MemberId == id);

            return member;
        }

        public Employees GetEmployeeInfo()
        {
            int id = Convert.ToInt32(_contextAccessor.HttpContext.User.Identity.Name);

            Employees employee = new OniqueContext().Employees.FirstOrDefault(e => e.EmployeeId == id);

            return employee;
        }
    }
}
