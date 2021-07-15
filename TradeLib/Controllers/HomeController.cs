using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

using Microsoft.AspNetCore.Authorization;

namespace TradeLib.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _db;
        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _db = context;
        }
        
        public IActionResult Index() => View();
        
        public IActionResult Privacy() => View();
        
        [Authorize]
        public IActionResult Person() => View(_db.Persons.ToList());
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}