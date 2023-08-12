using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Services
{
    public class DiscountService
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        private DiscountDao _dao;

        public DiscountService(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _dao = new DiscountDao(_context,_environment);
        }

        public void Create(BgDiscointCreateVM vm)
        {
            var discountName = _context.Discounts.FirstOrDefault(d => d.Title == vm.Title);
            if (discountName != null)
            {
                throw new Exception("已存在同名活動");
            }
            if (vm.BeginDate > vm.EndDate)
            {
                throw new Exception("開始日期大於結束日期");
            }
            else
            {
                _dao.Create(vm);
            }

        }

        public void Edit(BgDiscointCreateVM vm)
        {
            var discount = _context.Discounts.FirstOrDefault(d => d.Title == vm.Title);

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
                _dao.Edit(vm);
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
                _dao.AddToDiscount(productId, discountId);
            }
        }

        public void RemoveDiscountProuct(int id)
        {
            var discountId = _context.Products.Where(p=>p.ProductId==id).Select(p => p.DiscountId).FirstOrDefault();
            if (discountId == null)
            {
                throw new Exception("此商品尚未有優惠活動!");
            }
            else
            {
                _dao.RemoveDiscountProuct(id);
            }
        }
    }
}
