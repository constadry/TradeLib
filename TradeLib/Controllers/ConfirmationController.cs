using System;
using System.Data;
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
            foreach (var person in _db.Persons.Where(person => person.Email == address))
                person.Confirmed = true;
        }
    }
}