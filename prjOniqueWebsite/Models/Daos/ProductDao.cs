﻿using prjOniqueWebsite.Models.Dtos;
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
                         group od by new { p.Price, p.ProductName, p.PhotoPath ,p.ProductId} into grouped
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
                if(!dto.Color.Any(c=>c.ColorId==item.ColorId))
                dto.Color.Add(color);
                var size = _context.ProductSizes.FirstOrDefault(s => s.SizeId == item.SizeId);
                dto.Size.Add(size);
            }
            return dto;
        }

    }
}