using prjOniqueWebsite.Models.Daos;
using prjOniqueWebsite.Models.EFModels;
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
    }
}
