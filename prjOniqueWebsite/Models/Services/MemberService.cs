using prjOniqueWebsite.Controllers;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;

namespace prjOniqueWebsite.Models.Services
{
    public class MemberService
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao;
        public MemberService(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
        }
        public void MemberRegister(FMemberVM vm)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.Email);
            var memInPhone = _dao.GetMemberByPhone(vm.Phone);
            if (memInEmail != null)
            {
                throw new Exception("已有此信箱!");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            if (memInPhone != null)
            {
                throw new Exception("已有此電話號碼!");
            }
            if (Convert.ToDateTime(vm.DateOfBirth)>=DateTime.Now)
            {
                throw new Exception("輸入生日有誤");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            _dao.Register(vm);
        }
        public void MemberCreate(MemberVM vm)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.Email);
            var memInPhone = _dao.GetMemberByPhone(vm.Phone);
            if (memInEmail != null)
            {
                throw new Exception("已有此信箱!");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            if (memInPhone != null)
            {
                throw new Exception("已有此電話號碼!");
            }
            if (Convert.ToDateTime(vm.DateOfBirth) >= DateTime.Now)
            {
                throw new Exception("輸入生日有誤");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            _dao.CreateMember(vm);
        }
        public void MemberEdit(MemberEditVM vm)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.Email);
            if (memInEmail != null&& memInEmail.MemberId!=vm.MemberId)
            {
                throw new Exception("此信箱已被其他會員註冊，無法修改!");
            }
            var memInPhone = _dao.GetMemberByPhone(vm.Phone);
            if (memInPhone != null && memInPhone.MemberId!=vm.MemberId)
            {
                throw new Exception("已有此電話號碼!");
            }
            if(vm.Phone.Length > 10 || vm.Phone.Length < 10 )
            {
                throw new Exception("電話號碼為10碼!");
            }
            _dao.EditMember( vm);
        }
        public void FMemberEdit(FMemberEditVM vm)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.Email);
            if (memInEmail != null && memInEmail.MemberId != vm.MemberId)
            {
                throw new Exception("此信箱已被其他會員註冊，無法修改!");
            }
            var memInPhone = _dao.GetMemberByPhone(vm.Phone);
            if (memInPhone != null && memInPhone.MemberId != vm.MemberId)
            {
                throw new Exception("已有此電話號碼!");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 10)
            {
                throw new Exception("電話號碼為10碼!");
            }
            _dao.EditFMember(vm);
        }
        public void FMemberPasswordReset(FMemberPasswordVM vm , int loginMemId)
        {
             
            var originalPassword = _context.Members.Where(m=>m.MemberId== loginMemId).Select(m=>m.Password).FirstOrDefault();
            if(originalPassword!=vm.OldPassword)
            {
                throw new Exception("舊密碼不符合!");
            }
            _dao.FMemberPassword(vm,loginMemId);
        }

        public Members LineRegister(LineRegisterVM vm, LineLogin.LineProfile lineProfile)
        {
            var memInEmail = _dao.GetMemberByEmail(vm.Email);
            var memInPhone = _dao.GetMemberByPhone(vm.Phone);


            if (memInEmail != null && vm.Password != memInEmail.Password)
            {
                throw new Exception("此信箱已被註冊!");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            if (memInPhone != null)
            {
                throw new Exception("已有此電話號碼!");
            }
            if (vm.Phone.Length > 10 || vm.Phone.Length < 0)
            {
                throw new Exception("電話號碼為10碼!");
            }
            if (memInEmail != null && vm.Password == memInEmail.Password)
            {
                var member = _dao.BindLineLogin(vm, lineProfile);
                return member;
            }
            var newMember = _dao.LineRegister(vm, lineProfile);
            return newMember;
        }
    }
}
