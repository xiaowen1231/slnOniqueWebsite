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

        public List<ProductCardDto> HotTop4()
        {
            var query = (from p in _context.Products
                         join psd in _context.ProductStockDetails
                         on p.ProductId equals psd.ProductId
                         join od in _context.OrderDetails
                         on psd.StockId equals od.StockId
                         group od by new { p.Price, p.ProductName, p.PhotoPath } into grouped
                         orderby grouped.Sum(od => od.OrderQuantity) descending
                         select new ProductCardDto
                         {
                             ProductName = grouped.Key.ProductName,
                             Price = grouped.Key.Price,
                             PhotoPath = grouped.Key.PhotoPath
                         }).Take(4);

            return query.ToList();
        }
    }
}
