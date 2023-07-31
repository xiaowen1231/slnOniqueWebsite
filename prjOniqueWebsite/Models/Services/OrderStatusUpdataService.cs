using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

namespace prjOniqueWebsite.Models.Services
{
    public class OrderStatusUpdataService
    {
        private readonly ProductDao _dao;
        private readonly OniqueContext _context;
        public OrderStatusUpdataService(OniqueContext context)
        {
            _context = context;
            _dao = new ProductDao(_context);
        }
        
    }
}
