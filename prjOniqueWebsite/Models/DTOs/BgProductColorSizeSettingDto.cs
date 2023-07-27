namespace prjOniqueWebsite.Models.DTOs
{
    public class BgProductColorSizeSettingDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockId { get ; set; }   
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }

    }
}
