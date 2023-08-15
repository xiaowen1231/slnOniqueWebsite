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
                Price = (decimal)vm.Price,
                AddedTime = (DateTime)vm.AddedTime,
                ShelfTime = (DateTime)vm.ShelfTime,
                Description = vm.Description,
                ProductId = vm.ProductId,
                SupplierId = vm.SupplierId,
                PhotoPath = vm.PhotoPath,
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
            //else
            //{
            //    string fileName = product.ProductName + ".jpg";
            //    product.PhotoPath = fileName;
            //    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
            //    string photoDefault = Path.Combine(_environment.WebRootPath, "images", "uploads", "products", "default.jpg");
            //    System.IO.File.Copy(photoDefault, photoPath, true);
            //}
            _context.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProducts(BgProductsVM vm)
        {
            var products = _context.Products.FirstOrDefault(p => p.ProductId == vm.ProductId);
            products.ProductId = vm.ProductId;
            products.ProductName = vm.ProductName;
            products.Price = (decimal)vm.Price;
            products.AddedTime = (DateTime)vm.AddedTime;
            products.ShelfTime = (DateTime)vm.ShelfTime;
            products.Description = vm.Description;
            products.PhotoPath = vm.PhotoPath;
            products.ProductCategoryId = vm.ProductCategoryId;
            products.SupplierId = vm.SupplierId;
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
            _context.Update(products);
            _context.SaveChanges();
        }
    }
}

