using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Services
{
    public class BgProductService
    {
        private readonly OniqueContext _context;
        private IWebHostEnvironment _environment;
        public BgProductService(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }
        public void CreateDiscont(BgDiscointCreateVM vm)
        {
            var discountName = _context.Discounts.FirstOrDefault(d=>d.Title == vm.Title);
            if(discountName != null)
            {
                throw new Exception("已存在同名活動");
            }
            if (vm.BeginDate > vm.EndDate)
            {
                throw new Exception("開始日期大於結束日期");
            }
            else
            {
                new BgProductDao(_context,_environment).CreateDiscount(vm);
            }
        }

        public void UpdataDiscount(BgDiscointCreateVM vm)
        {
            var discount = _context.Discounts.FirstOrDefault(d=>d.Title==vm.Title);
            if (discount != null && discount.Id != vm.Id)
            {
                throw new Exception("已存在同名活動");
            }
            if (vm.BeginDate > vm.EndDate)
            {
                throw new Exception("開始日期大於結束日期");
            }
            else
            {
                new BgProductDao(_context, _environment).UpdataDiscount(vm);
            }
        }
        public void CreateProducts(BgProductsVM vm)
        {
            var productNameCheck = _context.Products.FirstOrDefault(p => p.ProductName == vm.ProductName);
            if (productNameCheck != null)
            {
                throw new Exception("已有相同的商品名稱存在，請確認後再試一次!");
            }
            if (vm.AddedTime > vm.ShelfTime)
            {
                throw new Exception("上架日期不可大於下架日期");
            }
            else
            {
                new BgProductDao(_context,_environment).CreateProducts(vm);
            }
        }

        public void AddToDiscount(int productId, int discountId)
        {
            int? discountIdInDb = _context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => p.DiscountId)
                .FirstOrDefault();
            if (discountIdInDb != null)
            {
                throw new Exception("此商品已被加入其他優惠!");
            }
            else
            {
                new BgProductDao(_context, _environment).AddToDiscount(productId, discountId);
            }
        }

        public void UpdataProducts(BgProductsVM vm)
        {
            var products = _context.Products.FirstOrDefault(p => p.ProductName == vm.ProductName);
            if(products != null && products.ProductId != vm.ProductId)
            {
                throw new Exception("已存在同名商品");
            }
            if(vm.AddedTime > vm.ShelfTime)
            {
                throw new Exception("上架日期不可大於下架日期");
            }
            else
            {
                new BgProductDao(_context,_environment).UpdataProducts(vm);
            }
        }
    }
}
