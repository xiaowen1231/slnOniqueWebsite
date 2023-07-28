using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.DTOs;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

namespace prjOniqueWebsite.Controllers
{
    public class BgProductsManageController : Controller
    {
        private readonly OniqueContext _context;

        public BgProductsManageController(OniqueContext context)
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
        public IActionResult BgCreateSize()
        {
            return View();
        }
        public IActionResult BgColorSizeSetting()
        {
            return View();
        }public IActionResult BgColorSizeDetails(int id)
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
        public IActionResult BgColorSizeSettingCreate()
        {
            BgProductColorSizeSettingDto query = (from psd in _context.ProductStockDetails
                                                  join p in _context.Products
                                                  on psd.ProductId equals p.ProductId
                                                  join c in _context.ProductColors
                                                  on psd.ColorId equals c.ColorId
                                                  join s in _context.ProductSizes
                                                  on psd.SizeId equals s.SizeId
                                                  select new BgProductColorSizeSettingDto
                                                  {
                                                      ProductId = p.ProductId,
                                                      ProductName = p.ProductName,
                                                      ColorName = c.ColorName,
                                                      SizeName = s.SizeName
                                                  }).FirstOrDefault();
            ViewBag.ProductName = query.ProductName;
            ViewBag.SizeName = query.SizeName;
            ViewBag.ColorName = query.ColorName;
            return View();

        }
        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
