using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Daos
{
    public class BgProductDao
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        public BgProductDao(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void CreateDiscount(BgDiscointCreateVM vm)
        {
            var discount = new Discounts
            {
                Title = vm.Title,
                Description = vm.Description,
                BeginDate = vm.BeginDate,
                EndDate = vm.EndDate,
                DiscountMethod = vm.DiscountMethod
            };
            if(vm.Photo!=null)
            {
                discount.PhotoPath = discount.Title + ".jpg";

                string photoPath = Path.Combine(_environment.WebRootPath, "images/DiscountPhoto", discount.PhotoPath);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public void UpdataDiscount(BgDiscointCreateVM vm)
        {
            var discount = _context.Discounts.FirstOrDefault(d=>d.Id == vm.Id);
            discount.Title = vm.Title;
            discount.Description = vm.Description;
            discount.BeginDate = vm.BeginDate;
            discount.EndDate = vm.EndDate;
            discount.DiscountMethod = vm.DiscountMethod;
            if (vm.Photo != null)
            {
                discount.PhotoPath = discount.Title + ".jpg";

                string photoPath = Path.Combine(_environment.WebRootPath, "images/DiscountPhoto", discount.PhotoPath);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            _context.SaveChanges();
        }
    }
}
