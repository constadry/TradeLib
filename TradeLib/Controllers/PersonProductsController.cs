using Microsoft.AspNetCore.Mvc;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class PersonProductsController : Controller
    {
        private readonly Context _db;

        public PersonProductsController(Context context)
        {
            _db = context;
        }

        public IActionResult PersonProducts() => View(_db);
    }
}