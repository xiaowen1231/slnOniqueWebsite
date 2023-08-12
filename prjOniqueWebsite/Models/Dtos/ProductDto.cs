﻿namespace prjOniqueWebsite.Models.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }
        public DateTime AddedTime { get; set; }
        public int SubQuantity { get; set; }
        public string catagoryName { get; set; }
        public int? DiscountId { get; set; }
        public decimal? DiscountMethod { get; set; }
        public decimal? DiscountPrice
        {
            get
            {
                if (DiscountMethod!.HasValue)
                {
                    return this.Price * this.DiscountMethod;
                }
                else { return null; }
            }
            
        }

    }
}
