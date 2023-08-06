using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using prjOniqueWebsite.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace prjOniqueWebsite.Models.Repositories
{
    public class ProductDao
    {
        private readonly OniqueContext _context;

        public ProductDao(OniqueContext context)
        {
            _context = context;
        }

        public List<ProductsListDto> NewArrivalsTop4()
        {
            var query = _context.Products
                .OrderByDescending(p => p.AddedTime)
                .Take(4)
                .Select(p => new ProductsListDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    PhotoPath = p.PhotoPath
                });

            return query.ToList();
        }

        public List<ProductsListDto> HotTop4()
        {
            var query = (from p in _context.Products
                         join psd in _context.ProductStockDetails
                         on p.ProductId equals psd.ProductId
                         join od in _context.OrderDetails
                         on psd.StockId equals od.StockId
                         group od by new { p.Price, p.ProductName, p.PhotoPath, p.ProductId } into grouped
                         orderby grouped.Sum(od => od.OrderQuantity) descending
                         select new ProductsListDto
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
            ProductStockDetails psd = _context.ProductStockDetails.Where(psd => psd.ProductId == productId
            && psd.ColorId == colorId
            && psd.SizeId == sizeId)
                .FirstOrDefault();

            return psd;
        }

        public void AddToCart(int stockId, int qty, Members member)
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
                       join pc in _context.ProductColors
                       on psd.ColorId equals pc.ColorId
                       join ps in _context.ProductSizes
                       on psd.SizeId equals ps.SizeId
                       where s.MemberId == member.MemberId
                       select new ShoppingCartDto
                       {
                           Product = p,
                           ShoppingCart = s,
                           StockId = psd.StockId,
                           PhotoPath = psd.PhotoPath,
                           ProductColors = pc,
                           ProductSizes = ps,
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
            var query = _context.ShoppingCart.FirstOrDefault(sc => sc.Id == shoppingCartId);
            _context.Remove(query);
            _context.SaveChanges();
        }

        public UpdateShoppingQtyVM GetCartItems(int memberId, int updateQty, int stockId)
        {
            var cart = _context.ShoppingCart.Where(sc => sc.MemberId == memberId && sc.StockId == stockId)
                .Select(sc => new UpdateShoppingQtyVM
                {
                    ShoppingCartId = sc.Id,
                    UpdateQty = sc.OrderQuantity + updateQty,
                    StockId = stockId
                })
                .FirstOrDefault();
            if (cart == null)
                return null;
            return cart;
        }

        public List<ShippingMethods> DisplayShippingMethod()
        {
            var query = from shippingMethod in _context.ShippingMethods
                        select shippingMethod;
            return query.ToList();
        }

        public List<PaymentMethods> DisplayPaymentMethods()
        {
            var query = from paymentMethod in _context.PaymentMethods
                        select paymentMethod;
            return query.ToList();
        }

        public List<ProductsListDto> SearchProductList(string keyword, string categoryName, string rank)
        {
            var query = from p in _context.Products
                        join psd in _context.ProductStockDetails
                        on p.ProductId equals psd.ProductId into psdGroup
                        from psd in psdGroup.DefaultIfEmpty()
                        join od in _context.OrderDetails
                        on psd != null ? psd.StockId : 0 equals od.StockId into odGroup
                        from od in odGroup.DefaultIfEmpty()
                        group od by new { p.ProductName, p.Price, p.PhotoPath, p.AddedTime, p.ProductCategory.CategoryName } into grouped
                        select new ProductsListDto
                        {
                            ProductName = grouped.Key.ProductName,
                            Price = grouped.Key.Price,
                            PhotoPath = grouped.Key.PhotoPath,
                            AddedTime = grouped.Key.AddedTime,
                            catagoryName = grouped.Key.CategoryName,
                            SubQuantity = grouped.Sum(x => x != null ? x.OrderQuantity : 0)
                        };

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.catagoryName.Contains(keyword) || p.ProductName.Contains(keyword));
            }
            else if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.catagoryName.Contains(categoryName));
            }

            List<ProductsListDto> datas = query.ToList();

            return SortProductList(datas, rank);
        }

        public List<ProductsListDto> SortProductList(List<ProductsListDto> datas, string rank)
        {
            if (rank == "newest" || string.IsNullOrEmpty(rank))
            {
                datas = datas.OrderByDescending(p => p.AddedTime).ToList();
            }
            if (rank == "sales")
            {
                datas = datas.OrderByDescending(p => p.SubQuantity).ToList();
            }
            if (rank == "priceDesc")
            {
                datas = datas.OrderByDescending(p => p.Price).ToList();
            }
            if (rank == "priceAsc")
            {
                datas = datas.OrderBy(p => p.Price).ToList();
            }
            return datas;
        }

        public List<string> GetCategories()
        {
            var query = _context.Categories.Select(c => c.CategoryName).ToList();
            return query;
        }
    }
}