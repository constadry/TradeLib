using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Context _db;

        public CategoryController(Context context)
        {
            _db = context;
        }
        
        [HttpGet]
        public IActionResult ShowCategory()
        {
            var categoryName = Request.QueryString.Value?.Split('=').LastOrDefault();
            var neededRecords = _db.Products.
                Where(product => product.Type == categoryName).
                ToList();
            return View(neededRecords);
        } 
    }
}