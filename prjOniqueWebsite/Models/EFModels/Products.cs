﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.EFModels
{
    public partial class Products
    {
        public Products()
        {
            ProductStockDetails = new HashSet<ProductStockDetails>();
        }

        public int ProductId { get; set; }
       
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTime AddedTime { get; set; }
        public DateTime ShelfTime { get; set; }
        public int SupplierId { get; set; }
        public int? DiscountId { get; set; }
        public string? PhotoPath { get; set; }

        public virtual Discounts Discount { get; set; }
        public virtual Categories ProductCategory { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductStockDetails> ProductStockDetails { get; set; }
    }
}