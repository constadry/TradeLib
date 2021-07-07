using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

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

                var message = new MimeMessage();
                var addressFrom = new MailboxAddress("Admin", "behappydtworry@gmail.com");
                message.From.Add(addressFrom);
                var addressTo = new MailboxAddress("User", ViewData["Email"].ToString());
                message.To.Add(addressTo);

                message.Subject = "Confirm registration";

                var body = new BodyBuilder {HtmlBody = "<a>Click here to confirm the registration on TradeLib</a>"};
                message.Body = body.ToMessageBody();

                var client = new SmtpClient();
                try
                {
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate("behappydtworry@gmail.com","$om&Vasily2_2");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    throw;
                }
                
                
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                //test page to show working registration method
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