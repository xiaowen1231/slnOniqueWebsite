using prjOniqueWebsite.Models.Infra;

namespace prjOniqueWebsite.Models.DTOs
{
    public class AddToCartDto
    {
        public AddToCartDto()
        {
            this.ApiResult = new ApiResult();
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PhotoPath { get; set; }
        public decimal Price { get; set; }

        public ApiResult ApiResult { get; set; }
    }
}
