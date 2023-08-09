﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace prjOniqueWebsite.Models.EFModels
{
    public partial class Discounts
    {
        public Discounts()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        [Display(Name ="活動名稱")]
        [Required(ErrorMessage ="活動名稱不可為空白")]
        public string Title { get; set; }
        [Display(Name = "活動說明")]
        [Required(ErrorMessage = "活動說明不可為空白")]
        public string Description { get; set; }
        [Display(Name = "折扣方式")]
        [Required(ErrorMessage = "折扣不可為空白")]
        public decimal DiscountMethod { get; set; }
        [Display(Name = "生效日期")]
        [Required(ErrorMessage = "生效日期不可為空白")]
        public DateTime BeginDate { get; set; }
        [Display(Name = "結束日期")]
        [Required(ErrorMessage = "結束日期不可為空白")]
        public DateTime EndDate { get; set; }
        public string PhotoPath { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}