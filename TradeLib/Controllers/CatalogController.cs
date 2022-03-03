using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class CatalogController : Controller
    {
        private readonly Context _db;

        public CatalogController(ILogger<CatalogController> logger, Context context)
        {
            _db = context;
        }

        public IActionResult Index() => View(_db.Products.ToList());

    }
}