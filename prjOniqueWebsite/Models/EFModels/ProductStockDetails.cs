﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjOniqueWebsite.Models.EFModels
{
    public partial class ProductStockDetails
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public int? ProductPhotoId { get; set; }

        public virtual ProductColors Color { get; set; }
        public virtual Products Product { get; set; }
        public virtual ProductPhotos ProductPhoto { get; set; }
        public virtual ProductSizes Size { get; set; }
    }
}