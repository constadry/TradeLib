using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class ConfirmationController : Controller
    {
        private readonly Context _db;
        public ConfirmationController(Context context)
        {
            _db = context;
        }
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
    }
}