using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.Services;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Controllers
{
    
    public class BgProductsManageController : Controller
    {
        private readonly OniqueContext _context;
        private readonly IWebHostEnvironment _environment;

        public BgProductsManageController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;            
            _environment = environment;
        }

        // GET: BgProductsManage
        public async Task<IActionResult> Index(string txtKeyword,int pageNumber = 1,int pageSize=10)
        {
            IQueryable<Products> query = _context.Products.Include(p => p.Discount).Include(p => p.ProductCategory).Include(p => p.Supplier);
            if (!string.IsNullOrEmpty(txtKeyword))
            {
                query = query.Where(p => p.ProductName.Contains(txtKeyword) ||
                                    p.ProductCategory.CategoryName.Contains(txtKeyword) ||
                                    p.Price.ToString().Contains(txtKeyword));
            }
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            int startIndex = (pageNumber - 1) * pageSize;
            var products = await query.Skip(startIndex)
                .Take(pageSize)
                .ToListAsync();
            var viewModel = new BgProductsPagingVM
            {
                Products = products,
                PagingInfo = new PagingInfo
                {
                    CurrenPage = pageNumber,
                    ItemsPerPage = pageSize,
                    TotalItems = totalCount,
                    TotalPages = totalPages
                }
            };           
            return View(viewModel);
        }       

        // GET: BgProductsManage/Create
        public IActionResult Create()
        {
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "Description");
            ViewData["ProductCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName");
            return View();
        }

        // POST: BgProductsManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create(BgProductsVM vm)
        {
            if (ModelState.IsValid == false)
            {   
                return View(vm);
            }
            try
            {
                new BgProductService(_context, _environment).CreateProducts(vm);
                return RedirectToAction("Index");
            }           
            catch(Exception ex)
            {
                ModelState.AddModelError("", "新增商品失敗!" + ex.Message);
                return View(vm);
            }
            
        }

        // GET: BgProductsManage/Edit/5
        public IActionResult Edit(int? id)
        {
            var products = _context.Products.Where(d => d.ProductId == id).Select(d => new BgProductsVM 
            {
            ProductId=d.ProductId,
            ProductName=d.ProductName,
            Price=d.Price,
            AddedTime=d.AddedTime,
            ShelfTime=d.ShelfTime,
            Description=d.Description,
            PhotoPath=d.PhotoPath,
            }).FirstOrDefault();
            //if (id == null || _context.Products == null)
            //{
            //    return NotFound();
            //}

            //var products = await _context.Products.FindAsync(id);
            //if (products == null)
            //{
            //    return NotFound();
            //}
            //ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "Description", products.DiscountId);
            //ViewData["ProductCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", products.ProductCategoryId);
            //ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName", products.SupplierId);
            return View(products);
        }

        // POST: BgProductsManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductCategoryId,Price,Description,AddedTime,ShelfTime,SupplierId,DiscountId,PhotoPath")] Products products,IFormFile photo)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (photo != null)
                {
                    string fileName = products.ProductName + ".jpg";
                    products.PhotoPath = fileName;
                    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
                _context.Update(products);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "Description", products.DiscountId);
            ViewData["ProductCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", products.ProductCategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName", products.SupplierId);
            return View(products);
        }

        // GET: BgProductsManage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Discount)
                .Include(p => p.ProductCategory)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: BgProductsManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'OniqueContext.Products'  is null.");
            }
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);               
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult BgCreateColor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BgCreateColor(BgColorSizeSettingVM vm)
        {
            if (_context.ProductColors.Any(c => c.ColorName == vm.ColorName))
            {
                ModelState.AddModelError("ColorName", "該顏色已存在。");
                return View();
            }
            var color = new ProductColors()
            {
                ColorName = vm.ColorName
            };
            _context.ProductColors.Add(color);
            _context.SaveChanges();
            return RedirectToAction("BgCreateColor");
        }
        public IActionResult BgCreateSize()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BgCreateSize(BgColorSizeSettingVM vm)
        {
            if (_context.ProductSizes.Any(size => size.SizeName == vm.SizeName))
            {
                ModelState.AddModelError("SizeName", "該尺寸已存在。");
                return View();
            }
            
            var size = new ProductSizes()
            {                
                SizeName = vm.SizeName
            };
            _context.ProductSizes.Add(size);
            _context.SaveChanges();               
            
            return RedirectToAction("BgCreateSize");            
        }        
        public IActionResult BgColorSizeDetails(int id)
        {
            try
            {
                ProductDetailDto dto = new ProductDao(_context).GetProductDetail(id);
                return View(dto);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult BgColorSizeDetails(BgColorSizeSettingVM vm )
        {
            var productStockDetail = new ProductDao(_context).GetStockDetail(vm.ProductId, vm.ColorId, vm.SizeId);

            var query = _context.ProductStockDetails.Where(psd => psd.StockId == productStockDetail.StockId).FirstOrDefault();

            if(vm.Photo!=null)
            {
                string fileName = "StockId_" + productStockDetail.StockId + ".jpg";

                query.PhotoPath = fileName;

                string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
                using(var fileStream = new FileStream(photoPath, FileMode.Create))
                {
                    vm.Photo.CopyTo(fileStream);
                }

            }

            query.Quantity = vm.Quantity;
            _context.SaveChanges();
                       

            return RedirectToAction("Edit", new {  id =vm.ProductId});
        }
        public IActionResult BgColorSizeSettingCreate(int id)
        {
            var dto = _context.Products.Where(p=>p.ProductId==id).Select(p=>new BgProductColorSizeSettingDto {ProductId=id,ProductName=p.ProductName}).FirstOrDefault();
            dto.ProductSizes=_context.ProductSizes.ToList();
            dto.ProductColors=_context.ProductColors.ToList();
            return View(dto);                    
        }
        [HttpPost]
        public IActionResult BgColorSizeSettingCreate(BgColorSizeSettingVM vm)
        {
            // to do 邏輯判斷是否有重複要新增的顏色 尺寸
            try { 
                var BgCss = new ProductStockDetails()
                {
                    ProductId = vm.ProductId,
                    ColorId = vm.ColorId,
                    SizeId = vm.SizeId,
                    Quantity = 0,
                    PhotoPath= "default.jpg"
                };

                _context.ProductStockDetails.Add(BgCss);
                _context.SaveChanges();

                if (vm.Photo != null)
                {
                    string fileName = "StockId_" + BgCss.StockId + ".jpg";
                    var query = _context.ProductStockDetails.FirstOrDefault(p => p.StockId == BgCss.StockId);
                    query.PhotoPath = fileName;
                    _context.SaveChanges();

                    string photoPath = Path.Combine(_environment.WebRootPath, "images/uploads/products", fileName);
                    using (var fileStream = new FileStream(photoPath, FileMode.Create))
                    {
                        vm.Photo.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception ex) { return Content(ex.Message); }
            return RedirectToAction("Index");
        }
        public IActionResult BgDiscountCreate()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult BgDiscountCreate(BgDiscointCreateVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }
            try
            {
                new BgProductService(_context,_environment).CreateDiscont(vm);
                return RedirectToAction("BgDiscountManage");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("","新增優惠失敗!" + ex.Message);
                return View(vm);
            }
        }
        
        public IActionResult BgDiscountManage()
        {
            return View();
        }

        public IActionResult DiscountEdit(int id)
        {
            var discount = _context.Discounts.Where(d => d.Id == id)
                .Select(d => new BgDiscointCreateVM
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    BeginDate = d.BeginDate,
                    EndDate = d.EndDate,
                    PhotoPath = d.PhotoPath,
                    DiscountMethod = d.DiscountMethod
                }).FirstOrDefault();
            
            return View(discount);
        }

        [HttpPost]
        public IActionResult DiscountEdit(BgDiscointCreateVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm); 
            }
            try
            {
                new BgProductService(_context, _environment).UpdataDiscount(vm);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "修改失敗! " + ex.Message);
                return View(vm);
            }
            return RedirectToAction("BgDiscountManage");
        }
        public IActionResult DeleteSize(ProductSizes size)
        {            
            if(size!=null)
            {
                _context.Remove(size);
                _context.SaveChanges();
                return RedirectToAction("BgCreateSize");
            }
            return View();
        }
        public IActionResult DeleteColor(ProductColors color)
        {            
            if(color!=null)
            {
                _context.Remove(color);
                _context.SaveChanges();
                return RedirectToAction("BgCreateColor");
            }
            return View();
           
        }
        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
