using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;
using MailKit.Net.Smtp;
using MailKit;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System.Web;

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
        public IActionResult Registration() => View();
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

        private bool IsUserExist(string address)
        {
            return Enumerable.Any(_db.Persons, person => person.Email == address);
        }

        [HttpPost]
        public IActionResult Registration(Person person)
        {
            try
            {
                if (IsUserExist(person.Email)) return View("Index");
                person.Confirmed = false;
                _db.Add(person);
                _db.SaveChanges();
                SendMessage(person.Email);
                return View("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return View();
            }
        }
        
        [HttpPost]
        
        private static void SendMessage(string address)
        {
            var message = new MimeMessage();
            // var addressFrom = new MailboxAddress("TradeLib", "behappydtworry@gmail.com");
            var addressFrom = new MailboxAddress("TradeLib", "annbossova@gmail.com");

            message.From.Add(addressFrom);
            var addressTo = new MailboxAddress("User", address);
            message.To.Add(addressTo);

            message.Subject = "Confirm registration";

            var body = new BodyBuilder {HtmlBody =
                $"<a href= \" https://localhost:5001/Home/Confirmation?mail_ref={address} \">" +
                " Click here to confirm the registration on TradeLib" +
                "</a>"};
            message.Body = body.ToMessageBody();

            var client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                // client.Authenticate("behappydtworry@gmail.com","$om&Vasily2_2");
                client.Authenticate("annbossova@gmail.com","AlexBossov123412344321");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                throw;
            }

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}