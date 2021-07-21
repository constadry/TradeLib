using Microsoft.AspNetCore.Mvc;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class CartController : Controller
    {
        private readonly Context _db;

        public CartController(Context context)
        {
            _db = context;
        }

        public IActionResult CartView() => View(_db);
    }
}