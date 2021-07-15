using System;
using System.IO;
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

        public IActionResult CreateProduct() => View();

        public IActionResult ShowProduct() => View();

        public IActionResult CreateProduct(Product product, IFormFile uploadImage)
        {
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