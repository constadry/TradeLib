using System;
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
        
        public IActionResult Confirmation()
        {
            var address = Request.QueryString.Value?.Split('=').LastOrDefault();
            ConfirmRegistration(address);
            _db.SaveChanges();
            return View();
        }

        private void ConfirmRegistration(string address)
        {
            try
            {
                foreach (var person in _db.Persons)
                {
                    if (person.Email == address) person.Confirmed = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}, {address}");
                throw;
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}