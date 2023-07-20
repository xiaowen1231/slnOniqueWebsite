using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;

namespace prjOniqueWebsite.Models.Repositories
{
    public class ProductRepository
    {
        private readonly OniqueContext _context;
        public ProductRepository(OniqueContext context)
        {
            _context = context;
        }

    }
}
