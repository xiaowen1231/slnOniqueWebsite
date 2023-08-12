using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Models.Daos
{
    public class BgProductDao
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;
        public BgProductDao(OniqueContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void CreateProducts(BgProductsVM vm)
        {
            var product = new Products
            {
                ProductName = vm.ProductName,
                ProductCategoryId = vm.ProductCategoryId,
                Price = vm.Price,
                AddedTime = vm.AddedTime,
                ShelfTime = vm.ShelfTime,
                Description = vm.Description,
                ProductId = vm.ProductId,
                SupplierId = vm.SupplierId,
            };
            if (vm.Photo != null)
            {
                string fileName = product.ProductName + ".jpg";
                product.PhotoPath = fileName;
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            else
            {
                string fileName = product.ProductName + ".jpg";
                product.PhotoPath = fileName;
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
                string photoDefault = Path.Combine(_environment.WebRootPath, "images", "uploads", "products", "default.jpg");
                System.IO.File.Copy(photoDefault, photoPath, true);
            }
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdataProducts(BgProductsVM vm)
        {
            var products = _context.Products.FirstOrDefault(p => p.ProductId == vm.ProductId);
            products.ProductId = vm.ProductId;
            products.ProductName = vm.ProductName;
            products.Price = vm.Price;
            products.AddedTime = vm.AddedTime;
            products.ShelfTime = vm.ShelfTime;
            products.Description = vm.Description;
            products.PhotoPath = vm.PhotoPath;
            if (vm.Photo != null)
            {
                string fileName = products.ProductName + ".jpg";
                products.PhotoPath = fileName;
                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
                using (var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }
            }
            _context.SaveChanges();
        }
    }
}

