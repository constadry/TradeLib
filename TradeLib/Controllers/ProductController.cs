using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradeLib.Models;

namespace TradeLib.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private readonly Context _db;

        public ProductController(ILogger<ProductController> logger, Context context)
        {
            _logger = logger;
            _db = context;
        }
        
        [Authorize]
        public IActionResult CreateProduct() => View();
        public IActionResult ShowProduct(Guid? id)
        {
            if (id == null) return View();
            Console.WriteLine(id);
            foreach (var prod in _db.Products)
            {
                if (prod.Id == id)
                {
                    return View(prod);
                }
            }
            return View();
        }
        
        [Authorize][HttpPost]
        public IActionResult CreateProduct(Product product, IFormFile uploadImage)
        {
            var userEmail = User.Identity?.Name;
            var person = _db.Persons.ToList().FirstOrDefault(p => p.Confirmed && p.Email == userEmail);
            if (person == null) return RedirectToAction("ConfirmYourEmail", "Message");
            try
            {
                using (var ms = new MemoryStream())
                {
                    uploadImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    product.Image = fileBytes;
                }

                product.ImageName = uploadImage.FileName;

                _db.Products.Add(product);
                _db.SaveChanges();

                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return View();
            }
        }
    }
}