namespace prjOniqueWebsite.Models.Dtos
{
    public class DiscountDto
    {
        public int DiscountId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string PhotoPath { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string DiscountMethod { get; set; }
    }
}
