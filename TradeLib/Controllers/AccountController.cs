using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MailKit.Net.Smtp;
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

        public IActionResult Login() => View();
        
        [HttpPost][ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            foreach (var account in _db.Persons)
            {
                if (account.Email != loginModel.Email || account.Password != loginModel.Password) continue;
                if (!account.Confirmed) return RedirectToAction("ConfirmYourEmail", "Message");
                
                await Authenticate(account.Email);
                return RedirectToAction("Person", "Home");
            }
            return RedirectToAction("EnterExistData", "Message");
        }

        private bool IsUserExist(string address) => 
            Enumerable.Any(_db.Persons, person => person.Email == address);
        
        [HttpGet]
        public IActionResult Register() => View();
        
        [HttpPost]
        public IActionResult Register(RegisterModel person)
        {
            if (ModelState.IsValid)
            {
                if (!IsUserExist(person.Email))
                {
                    _db.Add(new Person
                    {
                        Email = person.Email, Name = person.Name, Password = person.Password, Confirmed = false
                    });
                    _db.SaveChanges();
                }

                var htmlBody = 
                    $"<a href= \" https://localhost:5001/Confirmation/Confirmation?mail_ref={person.Email} \">" +
                    "Click here to confirm the registration on TradeLib" +
                    "</a>";
                SendMessage(person.Email, htmlBody);
                return RedirectToAction("Person", "Home");
            }
            ModelState.AddModelError("", "Password or Email are mot correct");
            return View(person);
        }

        [HttpGet]
        public IActionResult ToRestore() => View();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToRestore(ToRestoreModel toRestoreModel)
        {
            if (!IsUserExist(toRestoreModel.Email)) return RedirectToAction("EnterExistData", "Message");
            //TODO: Check confirmation
            var htmlBody = 
                $"<a href= \" https://localhost:5001/Account/Restore?mail_ref={toRestoreModel.Email} \">" +
                " Click here to change your password" +
                "</a>";
            SendMessage(toRestoreModel.Email, htmlBody);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Restore()
        {
            ViewBag.Email = Request.QueryString.Value?.Split('=').LastOrDefault();
            return View();
        }

        [HttpPost]
        public IActionResult Restore(RestoreModel restoreModel)
        {
            foreach (var person in _db.Persons.Where(person => person.Email == restoreModel.Email))
                person.Password = restoreModel.Password;
            
            _db.SaveChanges();
            return RedirectToAction("Person", "Home");
        }

        public IActionResult EditProfile() => View();

        [HttpPost]
        public IActionResult EditProfile(EditProfileModel editProfileModel)
        {
            foreach (var person in _db.Persons)
            {
                if (person.Email != User.Identity?.Name) continue;
                person.Name = editProfileModel.Name;
            }
            _db.SaveChanges();
            return RedirectToAction("Person", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

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

        private static void SendMessage(string address, string htmlBody)
        {
            var message = new MimeMessage();
            // var addressFrom = new MailboxAddress("TradeLib", "behappydtworry@gmail.com");
            var addressFrom = new MailboxAddress("TradeLib", "annbossova@gmail.com");
            message.From.Add(addressFrom);
            var addressTo = new MailboxAddress("User", address);
            message.To.Add(addressTo);

            message.Subject = "Confirm registration";

            var body = new BodyBuilder 
            {HtmlBody = htmlBody};
            message.Body = body.ToMessageBody();

            var client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("behappydtworry@gmail.com","$om&Vasily2_2");
                // client.Authenticate("annbossova@gmail.com","AlexBossov123412344321");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}