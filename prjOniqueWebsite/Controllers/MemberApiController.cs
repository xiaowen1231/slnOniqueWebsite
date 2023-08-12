using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Diagnostics.Metrics;
using System.Drawing.Printing;
using System.Security.Cryptography;
using static prjOniqueWebsite.Controllers.OrderApiController;

namespace prjOniqueWebsite.Controllers
{
    public class MemberApiController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao dao = null;
        public MemberApiController(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            dao = new MemberDao(_context, _environment);
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
                            Gender = m.Gender ? "女" : "男",
                            MemberLevelName = ml.MemberLevelName,
                        };
            List<MemberListDto> dto = query.ToList();
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
        public IActionResult memberOrder(int MemberId,int pageNumber, int pageSize)
        {
            try
            {
                var dto = dao.GetMemberOrders(MemberId);
                var index = memberOrderIndex(dto, pageNumber, pageSize);
                return Json(index);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public IActionResult memberOrderIndex(List<MemberOrderDto>memberorder, int pageNumber, int pageSize)
        {
            var data = new MemberOrderGUIdto();
            var criteria = prepareCriteria(pageNumber);
            criteria.PageSize=pageSize;
            data.Criteria = criteria;

            int totalOrderCount = memberorder.Count();
            data.MemberOrderPaginationInfo = new MemberOrderPaginationInfo(totalOrderCount, criteria.PageSize, criteria.PageNumber, $"/MemberApi/memberOrder?pagenumber={pageNumber}&pagesize={pageSize}");
            data.MemberOrders= memberorder.Skip(criteria.recordStartIndex).Take(criteria.PageSize).ToList();
            return Json(data);
        }

        public class MemberOrderCriteria
        {
            private int _PageNumber = 1;

            public int PageNumber
            {
                get { return _PageNumber; }
                set { _PageNumber = value < 1 ? 1 : value; }
            }

            private int _PageSize = 10;

            public int PageSize
            {
                get { return _PageSize; }
                set { _PageSize = value < 1 ? 10 : value; }
            }

            public int recordStartIndex
            {
                get { return (PageNumber - 1) * PageSize; }
            }
        }
        public MemberOrderCriteria prepareCriteria(int pageNumber)
        {
            var result = new MemberOrderCriteria();
            result.PageNumber = pageNumber;

            return result;
        }
        public class MemberOrderGUIdto
        {
            public MemberOrderCriteria Criteria { get; set; }
            public  MemberOrderPaginationInfo MemberOrderPaginationInfo { get; set; }
            public List<MemberOrderDto> MemberOrders { get; set; }

        }
        public class MemberOrderPaginationInfo
        {
            public MemberOrderPaginationInfo(int totalRecords, int pageSize, int pageNumber, string urlTemplate)
            {
                TotalRecords = totalRecords;
                PageSize = pageSize;
                PageNumber = pageNumber;
                this.urlTemplate = urlTemplate;
            }
            private string urlTemplate;
            public string GetUrl(int pageNumber)
            {
                return string.Format(urlTemplate, pageNumber);
            }

            public int TotalRecords { get; set; }

            public int PageSize { get; set; }

            public int PageNumber { get; set; }

            public int Pages => (int)Math.Ceiling((double)TotalRecords / PageSize);

            public int PageItemCount => 5;

            public int PageBarStartNumber
            {
                get
                {
                    int startNumber = PageNumber - ((int)Math.Floor((double)this.PageItemCount / 2));
                    return startNumber < 1 ? 1 : startNumber;
                }
            }

            public int PageItemPrevNumber => (PageBarStartNumber <= 1) ? 1 : PageBarStartNumber - 1;

            public int PageBarItemCount => PageBarStartNumber + PageItemCount > Pages
                ? Pages - PageBarStartNumber + 1
                : PageItemCount;
            public int PageItemNextNumber => (PageBarStartNumber + PageItemCount >= Pages) ? Pages : PageBarStartNumber + PageItemCount;
        }
    }
}
