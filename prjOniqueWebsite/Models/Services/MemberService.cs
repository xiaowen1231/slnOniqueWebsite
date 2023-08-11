﻿using prjOniqueWebsite.Controllers;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;

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
            if(memInPhone != null)
            {
                throw new Exception("已有此電話號碼!");
            }
            _dao.Register(vm);
        }
    }
}
