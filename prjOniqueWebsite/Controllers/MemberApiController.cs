using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;

namespace prjOniqueWebsite.Controllers
{
    public class MemberApiController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao = null;
        public MemberApiController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult showMemberList()
        {
            List<MemberListDto> dto = _dao.GetMembers();
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
        public IActionResult LoadMemberLevel()
        {
            var memberLevel = from m in _context.MemberLevel
                              select m;
            return Json(memberLevel);
        }
        public IActionResult memberOrder(int MemberId)
        {
            var memberorder=_dao.GetMemberOrders(MemberId);
            return Json(memberorder);

        }
    }
}
