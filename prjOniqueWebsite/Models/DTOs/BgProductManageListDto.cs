using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.DTOs
{
    public class BgProductManageListDto
    {
        public BgProductManageListDto()
        {
            products = new List<Products>();
        }
        public List<Products> products = null; 
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Discont { get; set; }
      
    }
}
