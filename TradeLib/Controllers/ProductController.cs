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

        public IActionResult ShowProduct(Guid? id)
        {
            if (id == null) return View();
            var product = new Product();
            foreach (var prod in _db.Products)
            {
                if (prod.Id != id) continue;
                prod.VisitCount++;
                product = prod;
            }
            
            _db.SaveChanges();
            return View(product);
        }
        
        [Authorize]
        public IActionResult AddToCart(Guid? id)
        {
            if (id == null) return Content("Id - null??");

            foreach (var person in _db.Persons)
            {
                if (person.Email != User.Identity?.Name) continue;
                var cartPosition = new CartPositions
                {
                    PersonId = person.Id,
                    ProductId = id
                };
                if (IsDuplicate(cartPosition)) return RedirectToAction("Index", "Catalog"); 
                _db.CartPositions.Add(cartPosition);
            }
            _db.SaveChanges();
            return RedirectToAction("Index", "Catalog");
        }

        private bool IsDuplicate(CartPositions comparedPosition)
        {
            var cartPosition = _db.CartPositions.ToList().FirstOrDefault(
                cart => cart.PersonId == comparedPosition.PersonId &&
                        cart.ProductId == comparedPosition.ProductId
            );
            return cartPosition != null;
        }
        
        [Authorize]
        public IActionResult CreateProduct() => View();

        [Authorize]
        [HttpPost]
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

                var personProductsModel = new PersonProductsModel {ProductId = product.Id, PersonId = person.Id};
                _db.PersonProductsModels.Add(personProductsModel);
                _db.SaveChanges();

                return View("ShowProduct", product);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return View();
            }
        }

        [Authorize]
        public IActionResult EditProduct(Guid? id)
        {
            try
            {
                if (id is null) throw new Exception("Name is null");
                var product = _db.Products.FirstOrDefault(prod => prod.Id == id);
                if(product is null) throw new Exception("DB hasn't this id");
                
                return View(product);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditProduct(Product product, IFormFile uploadImage)
        {
            foreach (var prod in _db.Products.Where(p => p.Id == product.Id))
            {
                prod.Name = product.Name; 
                prod.Type = product.Type; 
                prod.Description = product.Description; 
                prod.Price = product.Price;
                if (uploadImage is not null)
                {
                    using (var ms = new MemoryStream())
                    {
                        uploadImage.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        prod.Image = fileBytes;
                    }
                }
            }

            _db.SaveChanges();
            return RedirectToAction("ShowProduct", "Product", product);
        }


        [Authorize]
        public IActionResult DeleteProduct(Guid? id)
        {
            var product = _db.Products.FirstOrDefault(prod => prod.Id == id);
            
            if (product is null)
                throw new Exception("Impossible");
            
            _db.Products.Remove(product);

            var ides = _db.CartPositions.Where(identifier => identifier.ProductId == product.Id).ToList();
            foreach (var identifier in ides)
            {
                _db.CartPositions.Remove(identifier);
            }
            
            var idesProduct = _db.PersonProductsModels.Where(identifier => identifier.ProductId == product.Id).ToList();
            foreach (var identifier in idesProduct)
            {
                _db.PersonProductsModels.Remove(identifier);
            }
            
            _db.SaveChanges();
            return RedirectToAction("Index", "Catalog");
        }
    }
}