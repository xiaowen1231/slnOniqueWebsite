using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
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
        MemberDao _dao;
        private readonly UserInfoService _userInfoService;
        public MemberApiController(OniqueContext context, IWebHostEnvironment environment, UserInfoService userInfoService)
        {
            _context = context;
            _environment = environment;
            _dao = new MemberDao(_context, _environment);
            _userInfoService = userInfoService;
        }
        public IActionResult ToggleCollectProduct(int productId)
        {
            Members member = _userInfoService.GetMemberInfo();
            var collectInDb = _context.Collect.Where(c => c.MemberId == member.MemberId && c.ProductId == productId);
            var reult = new ApiResult();
            try
            {
                if (collectInDb.Count()<=0)
                {
                    var collect = new Collect
                    {
                        MemberId = member.MemberId,
                        ProductId = productId,
                    };

                    _context.Collect.Add(collect);
                    _context.SaveChanges();

                    reult.StatusCode = 200;
                    reult.StatusMessage = "已加入收藏商品";

                    return Json(reult);
                }
                else
                {
                    _context.Collect.Remove(collectInDb.First());
                    _context.SaveChanges();

                    reult.StatusCode = 200;
                    reult.StatusMessage = "已移除此收藏商品";

                    return Json(reult);
                }
            }
            catch (Exception ex)
            {
                reult.StatusCode = 500;
                reult.StatusMessage = "收藏失敗!";

                return Json(reult);
            }
        }
        public IActionResult CollectProductList(int MemberId)
        {
            var dto = _dao.GetCollects(MemberId);
            return Json(dto);
        }
        public IActionResult CollectItems()
        {
            Members member = _userInfoService.GetMemberInfo();

            var cart = _dao.CollectItems(member);

            return Json(cart.Count);
        }
        public IActionResult GetFMemberPhoto()
        {
            Members member = _userInfoService.GetMemberInfo();
            var dto = _context.Members.Where(m => m.MemberId == member.MemberId).Select(m => new { photoPath = m.PhotoPath }).FirstOrDefault();

            return Json(dto);
        }
        public IActionResult UploadPhoto(IFormFile photo)
        {
            try
            {
                var mem = _userInfoService.GetMemberInfo();
                var memInDb = _context.Members.FirstOrDefault(m => m.MemberId == mem.MemberId);

                string fileName = $"MemberId_{mem.MemberId}.jpg";

                memInDb.PhotoPath = fileName;
                _context.SaveChanges();

                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/members", fileName);

                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }

                var result = new ApiResult
                {
                    StatusCode = 200,
                    StatusMessage = "更新大頭貼成功"
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                var result = new ApiResult
                {
                    StatusCode = 500,
                    StatusMessage = "更新大頭貼失敗"
                };
                return Json(result);
            }
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
        public ActionResult Delete(int id)
        {
            try
            {
                var member = _context.Members.FirstOrDefault(m => m.MemberId == id);
                if (member != null)
                {
                    _context.Members.Remove(member);
                }
                _context.SaveChanges();
                var result = new ApiResult { StatusCode = 200, StatusMessage = "刪除資料成功!" };
                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new ApiResult { StatusCode = 500, StatusMessage = "刪除資料失敗!" };
                return Json(result);

            }

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
        public IActionResult memberOrder(int MemberId, int pageNumber, int pageSize)
        {
            try
            {
                var dto = _dao.GetMemberOrders(MemberId);
                var index = memberOrderIndex(dto, pageNumber, pageSize);
                return Json(index);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public IActionResult memberOrderIndex(List<MemberOrderDto> memberorder, int pageNumber, int pageSize)
        {
            var data = new MemberOrderGUIdto();
            var criteria = prepareCriteria(pageNumber);
            criteria.PageSize = pageSize;
            data.Criteria = criteria;

            int totalOrderCount = memberorder.Count();
            data.MemberOrderPaginationInfo = new MemberOrderPaginationInfo(totalOrderCount, criteria.PageSize, criteria.PageNumber, $"/MemberApi/memberOrder?pagenumber={pageNumber}&pagesize={pageSize}");
            data.MemberOrders = memberorder.Skip(criteria.recordStartIndex).Take(criteria.PageSize).ToList();
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
            public MemberOrderPaginationInfo MemberOrderPaginationInfo { get; set; }
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
