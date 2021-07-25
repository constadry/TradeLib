using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class SearchController : Controller
    {
        private readonly Context _db;
        public SearchController(Context context)
        {
            _db = context;
        }
        [HttpPost]
        public IActionResult Search(SearchModel searchAccess)
        {
            var neededRecords = _db.Products.
                Where(product => product.Name == searchAccess.SearchInput).
                ToList();
            return View(neededRecords);
        }
    }
}