using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.Repositories
{
    public class ProductDao
    {
        private readonly OniqueContext _context;

        public ProductDao(OniqueContext context)
        {
            _context = context;
        }

        public List<ProductCardDto> NewArrivalsTop4()
        {
            var query = _context.Products
                .OrderByDescending(p => p.AddedTime)
                .Take(4)
                .Select(p => new ProductCardDto
                {
                    ProductName = p.ProductName,
                    Price = p.Price,
                    PhotoPath = p.PhotoPath
                });

            return query.ToList();
        }
    }
}
