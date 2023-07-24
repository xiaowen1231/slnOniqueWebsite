using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Controllers
{
    public class MemberApiController : Controller
    {
        private readonly OniqueContext _context;
        public MemberApiController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult showMemberList()
        {
            var query = from m in _context.Members
                        join ml in _context.MemberLevel
                        on m.MemberLevel equals ml.MemberLevelId
                        select new MemberListDto
                        {
                            MemberId = m.MemberId,
                            PhotoPath = m.PhotoPath,
                            Name = m.Name,
                            Phone = m.Phone,
                            Email = m.Email,
                            DateOfBirth = m.DateOfBirth,
                            Gender = m.Gender? "女":"男",
                            MemberLevelName = ml.MemberLevelName,
                        };
            List<MemberListDto> dto = query.ToList();  
            return Json(dto);
        }
    }
}
