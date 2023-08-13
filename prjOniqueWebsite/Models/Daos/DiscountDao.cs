using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Daos
{
    public class DiscountDao
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        public DiscountDao(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;

            _environment = environment;

        }
        public void Create(BgDiscointCreateVM vm)
        {
            var discount = new Discounts
            {
                Title = vm.Title,
                Description = vm.Description,
                BeginDate = (DateTime)vm.BeginDate,
                EndDate = (DateTime)vm.EndDate,
                DiscountMethod = (decimal)vm.DiscountMethod
            };
            if (vm.Photo != null)
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

        public void Edit(BgDiscointCreateVM vm)
        {
            var discount = _context.Discounts.FirstOrDefault(d => d.Id == vm.Id);
            discount.Title = vm.Title;
            discount.Description = vm.Description;
            discount.BeginDate = (DateTime)vm.BeginDate;
            discount.EndDate = (DateTime)vm.EndDate;
            discount.DiscountMethod = (decimal)vm.DiscountMethod;
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

        public void AddToDiscount(int productId, int discountId)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);

            product.DiscountId = discountId;

            _context.SaveChanges();
        }

        public List<DiscountProductListDto> GetDiscountProducts(int discountId)
        {
            var dto = from p in _context.Products
                      join d in _context.Discounts
                      on p.DiscountId equals d.Id
                      where p.DiscountId == discountId
                      orderby  p.AddedTime descending
                      select new DiscountProductListDto
                      {
                          ProductId = p.ProductId,
                          PhotoPath = p.PhotoPath,
                          ProductName = p.ProductName,
                          Price = p.Price,
                          DiscountMethod = d.DiscountMethod
                      };

            return dto.ToList();
        }

        public void RemoveDiscountProuct(int id)
        {
            var product = _context.Products.First(p=>p.ProductId == id);
            product.DiscountId = null;
            _context.SaveChanges();
        }

        public void DeleteDiscount(int id)
        {
            var discount = _context.Discounts.FirstOrDefault(x => x.Id == id);
            var products = _context.Products.Where(p => p.DiscountId == discount.Id);

            foreach(var item in products)
            {
                item.DiscountId = null;
            }
            
            _context.SaveChanges();
            _context.Discounts.Remove(discount);
            _context.SaveChanges();
        }

        public List<DiscountDto> GetDiscountList()
        {
            var dto = _context.Discounts.Select(d => new DiscountDto
            {
                DiscountId = d.Id,
                Title = d.Title,
                Description = d.Description,
                BeginDate = d.BeginDate.ToShortDateString(),
                EndDate = d.EndDate.ToShortDateString(),
                DiscountMethod = (d.DiscountMethod * 100).ToString("0") + " %OFF",
                PhotoPath = d.PhotoPath,
            }).ToList();

            return dto;
        }
    }
}
