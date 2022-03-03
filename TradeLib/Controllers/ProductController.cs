using System;
using System.Diagnostics.CodeAnalysis;
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
        private readonly Context _db;

        public ProductController(Context context)
        {
            _db = context;
        }

        public IActionResult ShowProduct(Guid? id)
        {
            if (id is null) return View();
            
            var product = new Product();
            foreach (var prod in _db.Products.Where(prod => prod.Id == id))
            {
                prod.VisitCount++;
                product = prod;
            }
            
            _db.SaveChanges();
            return View(product);
        }
        
        [Authorize]
        public IActionResult AddToCart(Guid? id)
        {
            try
            {
                if (id is null) throw new Exception("Id is null");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

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
        public IActionResult CreateProduct(Product product, IFormFile? uploadImage)
        {
            var userEmail = User.Identity?.Name;
            var person = _db.Persons.ToList().FirstOrDefault(p => p.Confirmed && p.Email == userEmail);
            if (person == null) return RedirectToAction("ConfirmYourEmail", "Message");
            try
            {
                if (uploadImage is not null)
                {
                    using (var ms = new MemoryStream())
                    {
                        uploadImage.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        product.Image = fileBytes;
                    }
                    product.ImageName = uploadImage.FileName;
                }
                 // TODO: default image
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
                if (id is null) throw new Exception("Id is null");
                var product = _db.Products.FirstOrDefault(prod => prod.Id == id);
                if(product is null) throw new Exception("DB hasn't this id");
                
                return View(product);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
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
                
                if (uploadImage is null) continue;
                
                using var ms = new MemoryStream();
                uploadImage.CopyTo(ms);
                var fileBytes = ms.ToArray();
                prod.Image = fileBytes;
            }

            _db.SaveChanges();
            return RedirectToAction("ShowProduct", "Product", product);
        }
        
        [Authorize]
        public IActionResult DeleteProduct(Guid? id)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(prod => prod.Id == id);
            
                if (product is null)
                    throw new Exception("DB hasn't this product");
            
                _db.Products.Remove(product);
                foreach (var cartPosition in _db.CartPositions.Where(cartPosition => cartPosition.ProductId == product.Id))
                    _db.CartPositions.Remove(cartPosition);
                
                foreach (var personProductsModel in _db.PersonProductsModels.Where(personProductsModel => personProductsModel.ProductId == product.Id))
                    _db.PersonProductsModels.Remove(personProductsModel);

                _db.SaveChanges();
                return RedirectToAction("PersonProducts", "PersonProducts");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error");
            }
        }
    }
}