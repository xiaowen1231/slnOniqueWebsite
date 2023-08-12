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

        public IActionResult List(string keyword, string categoryName,string rank,int pageNumber)
        {
            ViewBag.CategoryName=categoryName;
            ViewBag.Keyword = keyword;
            ViewBag.PageNumber = pageNumber;
            return View();
        }

        public IActionResult Detail(int id)
        {
            try
            {
                var dto = new ProductDao(_context).ProductInfo(id);
                return View(dto);
            }
            catch
            {
                return RedirectToAction("NotReady");
            }
        }

        public IActionResult NotReady()
        {
            return View();
        }
    }
}
