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
                new BgProductDao(_context,_environment).UpdateProducts(vm);
            }
        }
    }
}
