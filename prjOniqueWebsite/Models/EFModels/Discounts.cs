﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace prjOniqueWebsite.Models.EFModels
{
    public partial class Discounts
    {
        public Discounts()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DiscountMethod { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PhotoPath { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}