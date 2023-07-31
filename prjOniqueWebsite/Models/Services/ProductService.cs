using prjOniqueWebsite.Models.EFModels;
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
            var cartInDb = _dao.GetCartItems(member.MemberId, qty, stockId);
            if(cartInDb != null)
            {
                cartInDb = new ShoppingCartService(_context).UpdateOrderQty(cartInDb);
                if(cartInDb.StatusCode==200)
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
                    _dao.AddToCart(stockId,qty, member);
                    newCart.StatusCode= 200;
                    newCart.Message = "加入購物車成功";
                    return newCart;

                }
            }
        }
    }
}
