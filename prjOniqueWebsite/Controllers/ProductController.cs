﻿using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.Dtos;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.Repositories;

namespace prjOniqueWebsite.Controllers
{
    public class ProductController : Controller
    {
        private readonly OniqueContext _context;

        public ProductController(OniqueContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            try { 
            ProductDetailDto dto = new ProductDao(_context).GetProductDetail(id);
            return View(dto);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}