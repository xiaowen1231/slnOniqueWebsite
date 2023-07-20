using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.Repositories
{
    public class ProductRepository
    {
        public readonly OniqueContext _context;
        public ProductRepository(OniqueContext context)
        {
            _context = context;
        }

        public ProductCardDto Get(int id = 24)
        {
            var prod = _context.Products.Where(p => p.ProductId == id).Select(p => new ProductCardDto
            {
                ProductName = p.ProductName,
                Price = p.Price,
                PhotoPath = p.PhotoPath
            }).FirstOrDefault();
            if (prod != null)
                return prod;
            return null;
        }
    }
}
