using System;
using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class Product
    {
        [Required][Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Type { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
        
        [Required]
        public string Price { get; set; }
    }
}