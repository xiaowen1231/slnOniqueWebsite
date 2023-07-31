using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using prjOniqueWebsite.Models.ViewModels;

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
                    Id = p.ProductId,
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
                         group od by new { p.Price, p.ProductName, p.PhotoPath, p.ProductId } into grouped
                         orderby grouped.Sum(od => od.OrderQuantity) descending
                         select new ProductCardDto
                         {
                             Id = grouped.Key.ProductId,
                             ProductName = grouped.Key.ProductName,
                             Price = grouped.Key.Price,
                             PhotoPath = grouped.Key.PhotoPath
                         }).Take(4);

            return query.ToList();
        }

        public ProductDetailDto GetProductDetail(int id)
        {

            ProductDetailDto dto = new ProductDetailDto();

            dto.products = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (dto.products == null)
                throw new Exception("請確認產品名稱");

            dto.PSD_List = _context.ProductStockDetails.Where(psd => psd.ProductId == dto.products.ProductId).ToList();

            if (dto.PSD_List.Count == 0)
                throw new Exception("請先新增庫存資訊");

            foreach (var item in dto.PSD_List)
            {
                var color = _context.ProductColors.FirstOrDefault(c => c.ColorId == item.ColorId);
                if (!dto.Color.Any(c => c.ColorId == item.ColorId))
                    dto.Color.Add(color);
                var size = _context.ProductSizes.FirstOrDefault(s => s.SizeId == item.SizeId);
                dto.Size.Add(size);
            }
            return dto;
        }

        public List<ProductSizes> GetStockSize(int id, int colorId)
        {
            var sizes = (from p in _context.Products
                         join psd in _context.ProductStockDetails
                         on p.ProductId equals psd.ProductId
                         join pc in _context.ProductColors
                         on psd.ColorId equals pc.ColorId
                         join ps in _context.ProductSizes
                         on psd.SizeId equals ps.SizeId
                         where p.ProductId == id && pc.ColorId == colorId
                         select new ProductSizes
                         {
                             SizeId = ps.SizeId,
                             SizeName = ps.SizeName
                         });

            return sizes.ToList();
        }
        public List<ProductColors> GetStockColor(int id)
        {
            var color = (from p in _context.Products
                         join psd in _context.ProductStockDetails
                         on p.ProductId equals psd.ProductId
                         join pc in _context.ProductColors
                         on psd.ColorId equals pc.ColorId
                         where p.ProductId == id
                         group pc by new { pc.ColorId, pc.ColorName } into grouped
                         select new ProductColors
                         {
                             ColorId = grouped.Key.ColorId,
                             ColorName = grouped.Key.ColorName
                         });

            return color.ToList();
        }

        public ProductStockDetails GetStockDetail(int productId, int colorId, int sizeId)
        {
            ProductStockDetails psd = _context.ProductStockDetails.Where(psd=>psd.ProductId==productId
            &&psd.ColorId==colorId
            &&psd.SizeId==sizeId)
                .FirstOrDefault();

            return psd;
        }

        public void AddToCart(int stockId,int qty,Members member)
        {
            var shoppingCart = new ShoppingCart();
            shoppingCart.StockId = stockId;
            shoppingCart.MemberId = member.MemberId;
            shoppingCart.OrderQuantity = qty;

            _context.ShoppingCart.Add(shoppingCart);
            _context.SaveChanges();
        }

        public List<ShoppingCartDto> CartItems(Members member)
        {
            var cart = from s in _context.ShoppingCart
                       join psd in _context.ProductStockDetails
                       on s.StockId equals psd.StockId
                       join p in _context.Products
                       on psd.ProductId equals p.ProductId
                       join c in _context.ProductColors
                       on psd.ColorId equals c.ColorId
                       join ps in _context.ProductSizes
                       on psd.SizeId equals ps.SizeId
                       where s.MemberId == member.MemberId
                       select new ShoppingCartDto
                       {
                           Product = p,
                           ProductStock = psd,
                           ShoppingCart = s,
                           Sizes = ps,
                           Colors = c
                       };

            return cart.ToList();
        }
        public void UpdateOrderQty(UpdateShoppingQtyVM vm)
        {
            ShoppingCart shoppingCart = _context.ShoppingCart.FirstOrDefault(psd => psd.Id == vm.ShoppingCartId);
            shoppingCart.OrderQuantity = vm.UpdateQty;
            _context.SaveChanges();

        }
        public ProductStockDetails GetProductStock(int stockId)
        {
            var productStockDetails = _context.ProductStockDetails.FirstOrDefault(psd => psd.StockId == stockId);

            return productStockDetails;
        }

        public void DeleteCartItem(int shoppingCartId)
        {
            var query = _context.ShoppingCart.FirstOrDefault(sc=>sc.Id == shoppingCartId);
            _context.Remove(query);
            _context.SaveChanges();
        }
    }
}