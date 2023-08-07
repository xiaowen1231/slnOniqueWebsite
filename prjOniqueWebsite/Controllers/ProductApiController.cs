using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;
using System.Collections.Generic;
using System.Text.Json;

namespace prjOniqueWebsite.Controllers
{
    public class ProductApiController : Controller
    {
        private readonly OniqueContext _context;
        ProductDao dao = null;

        public ProductApiController(OniqueContext context)
        {
            _context = context;
            dao = new ProductDao(_context);
        }

        public IActionResult NewArrivalsTop4()
        {
            List<ProductsListDto> dto = dao.NewArrivalsTop4();
            return Json(dto);
        }

        public IActionResult HotTop4()
        {
            List<ProductsListDto> dto = dao.HotTop4();
            return Json(dto);
        }

        public IActionResult GetStockSize(int id, int colorId)
        {
            List<ProductSizes> sizes = dao.GetStockSize(id, colorId);
            return Json(sizes);
        }

        public IActionResult GetStockColor(int id)
        {
            List<ProductColors> colors = dao.GetStockColor(id);
            return Json(colors);
        }

        public IActionResult GetStockDetail(int productId, int colorId, int sizeId)
        {
            ProductStockDetails stock = dao.GetStockDetail(productId, colorId, sizeId);
            return Json(stock);
        }

        public IActionResult AddToCart(int stockId, int qty)
        {
            string json = HttpContext.Session.GetString("Login");

            Members member = JsonSerializer.Deserialize<Members>(json);

            var service = new ProductService(_context);

            var vm = service.AddToCart(stockId, qty, member);

            return Json(vm);
        }
        public IActionResult CartItems()
        {
            string json = HttpContext.Session.GetString("Login");

            Members member = JsonSerializer.Deserialize<Members>(json);

            var cart = dao.CartItems(member);

            return Json(cart.Count);
        }
        public IActionResult CartList()
        {
            string json = HttpContext.Session.GetString("Login");
            Members member = JsonSerializer.Deserialize<Members>(json);
            var cart = dao.CartItems(member);

            return Json(cart);
        }

        public IActionResult ProductList(string keyword, string categoryName, string rank,int pageNumber)
        {
            var dtos = dao.SearchProductList(keyword, categoryName, rank);
            var datas = ProductListIndex(dtos, pageNumber);
            return Json(datas);
        }

        public IActionResult GetCategories()
        {
            var datas = dao.GetCategories();
            return Json(datas);
        }

        public IActionResult ProductListIndex(List<ProductsListDto> products, int pageNumber)
        {
            var data = new GUIdto();

            int pageSize = 16;

            var criteria = prepareCriteria(pageNumber);
            criteria.PageSize = pageSize;
            data.Criteria = criteria;

            int totalRecords = products.Count();
            data.Pagination = new PaginationInfo(totalRecords, pageSize, criteria.PageNumber, $"/ProductApi/ProductList?PageNumber={pageNumber}");

            data.Products = products.Skip(criteria.recordStartIndex).Take(criteria.PageSize).ToList(); ;

            return Json(data);
        }

        public class Criteria
        {
            private int _PageNumber = 1;

            public int PageNumber
            {
                get { return _PageNumber; }
                set { _PageNumber = value < 1 ? 1 : value; }
            }

            private int _PageSize = 12;

            public int PageSize
            {
                get { return _PageSize; }
                set { _PageSize = value < 1 ? 12 : value; }
            }

            public int recordStartIndex
            {
                get { return (PageNumber - 1) * PageSize; }
            }
        }

        public Criteria prepareCriteria(int pageNumber)
        {
            var result = new Criteria();
            result.PageNumber = pageNumber;
            return result;
        }

        public class GUIdto
        {
            public Criteria Criteria { get; set; }
            public PaginationInfo Pagination { get; set; }

            public List<ProductsListDto> Products { get; set; }

        }

        public class PaginationInfo
        {
            public PaginationInfo(int totalRecords, int pageSize, int pageNumber, string urlTemplate)
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
