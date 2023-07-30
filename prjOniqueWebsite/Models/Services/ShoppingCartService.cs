using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

namespace prjOniqueWebsite.Models.Services
{
    public class ShoppingCartService
    {
        private readonly ProductDao _dao;
        private readonly OniqueContext _context;
        public ShoppingCartService(OniqueContext context)
        {
            _context = context;
            _dao = new ProductDao(_context);
        }
        public ShoppingCart UpdateOrderQty(int stockId, int orderQty, int shoppingCartId)
        {
            var psdInDb = _dao.GetProductStock(stockId);

            if (orderQty <= 0)
            {
                throw new Exception("訂購數量不可小於1件!");
            }
            else if (orderQty > psdInDb.Quantity)
            {
                throw new Exception("訂購數量不可超過庫存數量!");
            }
            else
            {
                var cart = _dao.UpdateOrderQty(shoppingCartId, orderQty);
                return cart;
            }
        }

    }
}
