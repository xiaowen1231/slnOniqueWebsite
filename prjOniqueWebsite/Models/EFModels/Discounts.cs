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
            Orders = new HashSet<Orders>();
        }

        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountMethod { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}