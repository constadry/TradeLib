using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MailKit.Net.Smtp;
using TradeLib.ViewModels; 
using TradeLib.Models; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MimeKit;

namespace TradeLib.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _db;

        public AccountController(Context context) => _db = context;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            foreach (var account in _db.Persons)
            {
                if (account.Email != loginModel.Email || account.Password != loginModel.Password) continue;
                if (account.Confirmed) 
                {
                    //Authentication
                    await Authenticate(account.Email);

                    return RedirectToAction("Person", "Home");
                }
                else
                {
                    //TODO: Show message: "Please, confirm your account"
                    return Content("Please, confirm your account");
                }
            }
            //TODO: Show message: "Please, enter existence email and password"
            return Content("Please, enter existence email and password");
        }

        private bool IsUserExist(string address) => 
            Enumerable.Any(_db.Persons, person => person.Email == address);
        
        private static void SendMessage(string address)
        {
            var message = new MimeMessage();
            var addressFrom = new MailboxAddress("TradeLib", "behappydtworry@gmail.com");
            message.From.Add(addressFrom);
            var addressTo = new MailboxAddress("User", address);
            message.To.Add(addressTo);

            message.Subject = "Confirm registration";

            var body = new BodyBuilder 
            {HtmlBody =
                $"<a href= \" https://localhost:5001/Home/Confirmation?mail_ref={address} \">" +
                " Click here to confirm the registration on TradeLib" +
                "</a>"
            };
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
        }
        
        [HttpPost]
        public IActionResult Register(RegisterModel person)
        {
            if (ModelState.IsValid)
            {
                if (IsUserExist(person.Email)) return RedirectToAction("Index", "Home");
                _db.Add(new Person
                {
                    Email = person.Email, Name = person.Name, Password = person.Password, Confirmed = false
                });
                _db.SaveChanges();
                SendMessage(person.Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Password or Email are mot correct");
            }

            return View(person);
        }

        [HttpGet]
        public IActionResult Register() => View();

        private async Task Authenticate(string userEmail)
        {
            var claims = new List<Claim>
            {
                new (ClaimsIdentity.DefaultNameClaimType, userEmail)
            };

            var claimIdentity = new ClaimsIdentity
            (
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}