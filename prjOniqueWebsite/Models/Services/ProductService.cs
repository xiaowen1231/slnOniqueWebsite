using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Infra;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Services
{
    public class ProductService
    {
        private readonly OniqueContext _context;
        private readonly ProductDao _dao;
        public ProductService(OniqueContext context)
        {

            _context = context;
            _dao = new ProductDao(context);

        }
        public UpdateShoppingQtyVM AddToCart(int stockId, int qty, Members? member)
        {
            if (member == null) throw new Exception("員工無法使用購物車功能");

            var cartInDb = _dao.GetCartItems(member.MemberId, qty, stockId);
            if (cartInDb != null)
            {
                cartInDb = new ShoppingCartService(_context).UpdateOrderQty(cartInDb);
                if (cartInDb.StatusCode == 200)
                    cartInDb.Message = "此商品已存在在購物車，" + cartInDb.Message;
                return cartInDb;
            }
            else
            {
                var newCart = new UpdateShoppingQtyVM();
                var stockQty = _dao.GetProductStock(stockId).Quantity;
                if (qty > stockQty)
                {
                    newCart.StatusCode = 500;
                    newCart.Message = "庫存數量不足，無法加入購物車!";
                    return newCart;
                }
                else if (qty <= 0)
                {
                    newCart.StatusCode = 500;
                    newCart.Message = "商品訂購數不可小於一件!";
                    return newCart;

                }
                else
                {
                    _dao.AddToCart(stockId, qty, member);
                    newCart.StatusCode = 200;
                    newCart.Message = "加入購物車成功";
                    return newCart;

                }
            }
        }

        public AddToCartDto ShowProductInfo(int id)
        {
            AddToCartDto dto = new AddToCartDto();

            var checkHasStock = _dao.CheckProductStock(id);

            if (checkHasStock == null)
            {
                dto.ApiResult.StatusCode = 500;
                dto.ApiResult.StatusMessage = "商品正在準備中!";
                return dto;
            }
            else
            {
                dto.ApiResult.StatusCode = 200;
                dto.ApiResult.StatusMessage = "請選擇商品尺寸,顏色";
                dto.ProductName = checkHasStock.ProductName;
                dto.ProductId = checkHasStock.ProductId;
                dto.Price = checkHasStock.Price;
                dto.PhotoPath = checkHasStock.PhotoPath;
                return dto;
            }
        }

        public ProductDto ProductInfo(int id)
        {
            var stock = _context.ProductStockDetails.Where(psd => psd.ProductId == id);
            if (stock.Count() <= 0)
            {
                throw new Exception("商品尚未設定尺寸、顏色");
            }
            var dto = _dao.ProductInfo(id);
            return dto;
        }
    }
}
