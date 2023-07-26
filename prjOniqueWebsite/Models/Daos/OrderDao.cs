using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Daos
{
    public class OrderDao
    {
        private readonly OniqueContext _context;

        public OrderDao(OniqueContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 回傳全部的會員資料list
        /// </summary>
        /// <returns></returns>
        public List<OrderListDto> getOrderList()
        {
            var orderList = from o in _context.Orders
                            join os in _context.OrderStatus
                            on o.OrderStatusId equals os.StatusId
                            join m in _context.Members
                            on o.MemberId equals m.MemberId
                            join pm in _context.PaymentMethods
                            on o.PaymentMethodId equals pm.PaymentMethodId
                            select new OrderListDto
                            {
                                StatusName = os.StatusName,
                                OrderId = o.OrderId,
                                Name = m.Name,
                                ShippingDate = (DateTime)o.ShippingDate,
                                PaymentMethodName = pm.PaymentMethodName,
                                PhotoPath = m.PhotoPath
                            };
            return orderList.ToList();
        }
        public List<OrderProductsListDto> getProductDetail(int orderId)
        {
            var productDetail = from od in _context.OrderDetails
                                join psd in _context.ProductStockDetails
                                on od.StockId equals psd.StockId
                                join pc in _context.ProductColors
                                on psd.ColorId equals pc.ColorId
                                join ps in _context.ProductSizes
                                on psd.SizeId equals ps.SizeId
                                join p in _context.Products
                                on psd.ProductId equals p.ProductId
                                where od.OrderId == orderId
                                select new OrderProductsListDto
                                {
                                    ProductName = p.ProductName,
                                    SizeName = ps.SizeName,
                                    ColorName = pc.ColorName,
                                    OrderQuantity = od.OrderQuantity,
                                    Price = od.Price,
                                    PhotoPath = p.PhotoPath
                                };

            return productDetail.ToList();
        }
        public OrderShippingDetailDto getShippingDetail(int orderId)
        {
            var shippingDetail = from o in _context.Orders
                                 join os in _context.OrderStatus
                                 on o.OrderStatusId equals os.StatusId
                                 join sm in _context.ShippingMethods
                                 on o.MethodId equals sm.MethodId
                                 join pm in _context.PaymentMethods
                                 on o.PaymentMethodId equals pm.PaymentMethodId
                                 join m in _context.Members
                                 on o.MemberId equals m.MemberId
                                 where o.OrderId == orderId
                                 select new OrderShippingDetailDto
                                 {
                                     Name = m.Name,
                                     PhotoPath = m.PhotoPath,
                                     Phone = m.Phone,
                                     OrderId = o.OrderId,
                                     ShippingAddress = o.ShippingAddress,
                                     StatusName = os.StatusName,
                                     MethodName = sm.MethodName,
                                     PaymentMethodName = pm.PaymentMethodName,
                                     OrderDate = o.OrderDate,
                                     ShippingDate = o.ShippingDate,
                                     CompletionDate = o.CompletionDate,
                                 };
            return shippingDetail.FirstOrDefault();
        }

        public OrderStatusDto GetOrderStatus(int orderId)
        {
            var query = from o in _context.Orders
                         join os in _context.OrderStatus
                         on o.OrderStatusId equals os.StatusId
                         join pm in _context.PaymentMethods
                         on o.PaymentMethodId equals pm.PaymentMethodId
                         where o.OrderId == orderId
                         select new OrderStatusDto
                         {
                             OrderId = o.OrderId,
                             StatusName = os.StatusName,
                             StatusId = os.StatusId,
                             PaymentMethodId = pm.PaymentMethodId,

                         };

            return query.FirstOrDefault();
            
        }
            
                
                
            

            
            
            

        public List<OrderStatusDto> GetAllOrderStatus()
        {
            var status = from o in _context.Orders
                         join os in _context.OrderStatus
                         on o.OrderStatusId equals os.StatusId
                         join pm in _context.PaymentMethods
                         on o.PaymentMethodId equals pm.PaymentMethodId

                         select new OrderStatusDto
                         {
                             OrderId = o.OrderId,
                             StatusName = os.StatusName,
                             StatusId = os.StatusId,
                             PaymentMethodId = pm.PaymentMethodId,

                         };

            return status.ToList();

        }
    }
}
