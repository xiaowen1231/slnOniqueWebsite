using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;
using prjOniqueWebsite.Models.ViewModels;

namespace prjOniqueWebsite.Controllers
{
    public class BgProductsManageController : Controller
    {
        private readonly OniqueContext _context;        

        public BgProductsManageController(OniqueContext context,IWebHostEnvironment environment)
        {
            _context = context;            
        }

        // GET: BgProductsManage
        public async Task<IActionResult> Index()
        {
            var oniqueContext = _context.Products.Include(p => p.Discount).Include(p => p.ProductCategory).Include(p => p.Supplier);
            return View(await oniqueContext.ToListAsync());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductCategoryId,Price,Description,AddedTime,ShelfTime,SupplierId,DiscountId,PhotoPath")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "Description", products.DiscountId);
            ViewData["ProductCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", products.ProductCategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName", products.SupplierId);
            return View(products);
        }

        // GET: BgProductsManage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "Id", "Description", products.DiscountId);
            ViewData["ProductCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", products.ProductCategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName", products.SupplierId);
            return View(products);
        }

        // POST: BgProductsManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductCategoryId,Price,Description,AddedTime,ShelfTime,SupplierId,DiscountId,PhotoPath")] Products products)
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
        public IActionResult BgColorSizeSettingCreate(int id)
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
        //[HttpPost]
        //public IActionResult BgColorSizeSettingCreate(BgColorSizeSettingVM vm)
        //{
        //    var BgCss = new ProductStockDetails()
        //    {
        //        ProductId = vm.ProductId,
        //        ColorId = vm.ColorId,
        //        SizeId = vm.SizeId,
        //    };
        //    _context.ProductStockDetails.Add(BgCss);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public IActionResult BgDiscountCreate()
        {
            return View();
        }
        public IActionResult BgDiscountManage()
        {
            return View();
        }
        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
