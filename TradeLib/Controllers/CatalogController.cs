using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILogger<CatalogController> _logger;

        private readonly Context _db;

        public CatalogController(ILogger<CatalogController> logger, Context context)
        {
            _logger = logger;
            _db = context;
        }

        public IActionResult Index() => View(_db.Products.ToList());

        // public IActionResult Index(List<Product> products = null)
        // {
        //     Console.Write(products?.LastOrDefault()?.Name);
        //     products ??= _db.Products.ToList();
        //     return View(products);
        // }
    }
}