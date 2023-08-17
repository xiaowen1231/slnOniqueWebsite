using Humanizer.Localisation.TimeToClockNotation;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;
using System.Runtime.CompilerServices;

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
        /// filter by layer
        /// 依據搜尋的keyword回傳訂單資料(半成品)，再傳給下一個f()繼續篩選
        /// </summary>
        /// <returns></returns>
        public List<OrderListDto> SearchOrderList(string keyword, string sort, DateTime? startDate)
        {

            var query = from o in _context.Orders
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
                            OrderDate = (DateTime)o.OrderDate,
                            PaymentMethodName = pm.PaymentMethodName,
                            PhotoPath = m.PhotoPath
                        };
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(o => o.Name.Contains(keyword) || o.StatusName.Contains(keyword) || o.OrderId.Equals(keyword));
            }
            if (startDate != null)
            {
                query = query.Where(o => o.OrderDate > startDate);
            }

            List<OrderListDto> data = query.ToList();
            return SortOrderList(data, sort);
        }
        /// <summary>
        /// 接續搜尋回傳的資料，以此做分類
        /// </summary>
        /// <param name="data">回傳的資料</param>
        /// <param name="sort">分類的方式</param>
        /// <returns></returns>
        public List<OrderListDto> SortOrderList(List<OrderListDto> data, string sort)
        {
            if (sort == "OrderDateDescending" || string.IsNullOrEmpty(sort))//default setting
            {
                data = data.OrderByDescending(o => o.OrderDate).ToList();
            }
            if (sort == "OrderDateAscending")
            {
                data = data.OrderBy(o => o.OrderDate).ToList();
            }
            return data;
        }
        public List<OrderProductsListDto> getProductDetail(string orderId)
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
                                join o in _context.Orders
                                on od.OrderId equals o.OrderId
                                join sm in _context.ShippingMethods
                                on o.MethodId equals sm.MethodId
                                where od.OrderId == orderId
                                select new OrderProductsListDto
                                {
                                    ProductName = p.ProductName,
                                    SizeName = ps.SizeName,
                                    ColorName = pc.ColorName,
                                    OrderQuantity = od.OrderQuantity,
                                    Price = od.Price,
                                    PhotoPath = p.PhotoPath,
                                    TotalPrice = o.TotalPrice,
                                    MethodName = sm.MethodName,
                                    ProductId = p.ProductId,
                                };

            return productDetail.ToList();
        }
        public OrderShippingDetailDto getShippingDetail(string orderId)
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
                                     Recipient = o.Recipient,
                                     RecipientPhone = o.RecipientPhone,
                                     Remark = o.Remark,
                                     StatusName = os.StatusName,
                                     MethodName = sm.MethodName,
                                     PaymentMethodName = pm.PaymentMethodName,
                                     OrderDate = o.OrderDate,
                                     ShippingDate = o.ShippingDate,
                                     CompletionDate = o.CompletionDate,
                                     TotalPrice = o.TotalPrice,
                                 };
            return shippingDetail.FirstOrDefault();
        }

        public OrderStatusDto GetOrderStatus(string orderId)
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
                            PaymentMethodName = pm.PaymentMethodName
                        };

            return query.FirstOrDefault();

        }

        public IQueryable<OrderStatusDto> GetAllOrderStatus()
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
                             PaymentMethodName = pm.PaymentMethodName

                         };

            return status;

        }
        /// <summary>
        /// 取得處於某訂單狀態的訂單數量
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public int GetOrderStatusCount(int statusId)
        {
            var count = (from o in _context.Orders
                         where o.OrderStatusId == statusId
                         select o).Count();
            return count;
        }
        public string GetEmailByOrderId(string orderId)
        {

            var query = from o in _context.Orders
                        join m in _context.Members
                        on o.MemberId equals m.MemberId
                        where o.OrderId == orderId
                        select m.Email;

            string email = query.First().ToString();
            return email;


        }
        /// <summary>
        /// 有foreign key的資料刪除要注意，
        /// </summary>
        /// <param name="orderId"></param>
        public void DeleteOrder(string orderId)
        {
            var orderdetails = _context.OrderDetails.Where(o => o.OrderId == orderId);
            var order = _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();

            foreach (var o in orderdetails)
            {
                _context.OrderDetails.Remove(o);
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
        public SendHtmlEmailContent getEmailTemplateContent(string OrderId)
        {
            var query = from o in _context.Orders
                        where o.OrderId == OrderId
                        join m in _context.Members
                        on o.MemberId equals m.MemberId
                        join od in _context.OrderDetails
                        on o.OrderId equals od.OrderId
                        join sm in _context.ShippingMethods
                        on o.MethodId equals sm.MethodId
                        join pm in _context.PaymentMethods
                        on o.PaymentMethodId equals pm.PaymentMethodId
                        join os in _context.OrderStatus
                        on o.OrderStatusId equals os.StatusId
                        join psd in _context.ProductStockDetails
                        on od.StockId equals psd.StockId
                        join p in _context.Products
                        on psd.ProductId equals p.ProductId

                        select new SendHtmlEmailContent
                        {
                            
                            OrderId = OrderId,
                            OrderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            StatusName = os.StatusName,
                            MethodName = sm.MethodName,
                            PaymentMethodName = pm.PaymentMethodName,
                            Recipient = o.Recipient,
                            RecipientPhone = o.RecipientPhone,
                            ShippingAddress = o.ShippingAddress,
                            Remark = o.Remark,
                            TotalPrice = o.TotalPrice,
                            Email=m.Email
                        };
            var productsquery = from o in _context.Orders
                                where o.OrderId == OrderId
                                join od in _context.OrderDetails
                                on o.OrderId equals od.OrderId
                                join psd in _context.ProductStockDetails
                                on od.StockId equals psd.StockId
                                join p in _context.Products
                                on psd.ProductId equals p.ProductId
                                join ps in _context.ProductSizes
                                on psd.SizeId equals ps.SizeId
                                join pc in _context.ProductColors
                                on psd.ColorId equals pc.ColorId
                                select new SendHtmlEmailProduct
                                {
                                    ProductName = p.ProductName,
                                    Price = od.Price,
                                    OrderQuantity = od.OrderQuantity,
                                    SizeName = ps.SizeName,
                                    ColorName = pc.ColorName,
                                };
            var content = query.FirstOrDefault();

            content.Products = productsquery.ToList();
            return content;
        }
        public IEnumerable<SendHtmlEmailProduct> getHtmlEmailProducts(string OrderId)
        {
            var query = from o in _context.Orders
                        where o.OrderId == OrderId
                        join od in _context.OrderDetails
                        on o.OrderId equals od.OrderId
                        join psd in _context.ProductStockDetails
                        on od.StockId equals psd.StockId
                        join p in _context.Products
                        on psd.ProductId equals p.ProductId
                        join ps in _context.ProductSizes
                        on psd.SizeId equals ps.SizeId
                        join pc in _context.ProductColors
                        on psd.ColorId equals pc.ColorId
                        select new SendHtmlEmailProduct
                        {
                            ProductName = p.ProductName,
                            Price = od.Price,
                            OrderQuantity = od.OrderQuantity,
                            SizeName = ps.SizeName,
                            ColorName = pc.ColorName,
                        };
            return query.ToList();
        }
    }
}
