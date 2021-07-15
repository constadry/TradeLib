using Microsoft.AspNetCore.Mvc;

namespace TradeLib.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult ConfirmYourEmail() => View();
        public IActionResult EnterExistData() => View();
    }
}