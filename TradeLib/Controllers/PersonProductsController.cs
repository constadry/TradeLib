using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class PersonProductsController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private readonly Context _db;

        public PersonProductsController(ILogger<ProductController> logger, Context context)
        {
            _logger = logger;
            _db = context;
        }

        public IActionResult PersonProducts() => View(_db);
    }
}