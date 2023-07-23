﻿using Microsoft.AspNetCore.Mvc;
using prjOniqueWebsite.Models.EFModels;
using prjOniqueWebsite.Models.DTOs;

namespace prjOniqueWebsite.Controllers
{
    public class OrderController : Controller
    {
        private readonly OniqueContext _context;
        public OrderController(OniqueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
        
    }
}
