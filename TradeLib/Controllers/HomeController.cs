using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public Context db;
        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Persons.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Registration()
        {
            return View();
        }
        
        public IActionResult Person()
        {
            return View();
        }
        
        // Считывание данных из формы регистрации
        [HttpPost]
        public IActionResult Registration(Person person)
        {
            try
            {
                ViewData["Email"] = person.Email;
                ViewData["Name"] = person.Name;
                ViewData["Password"] = person.Password;
                db.Add(person);
                return View("Person");
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