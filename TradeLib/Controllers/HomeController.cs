using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Registration()
        {
            return View();
        }
        
        // Считывание данных из формы регистрации
        [HttpGet]
        public IActionResult Registration(Person person)
        {
            try
            {
                ViewData["Email"] = person.Email;
                ViewData["Password"] = person.Password;
                return View("Index");
            }
            catch
            {
                return View();
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}