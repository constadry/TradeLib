using System;
using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class Product
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Type { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        public byte[] Image { get; set; }
        public string ImageName { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public int VisitCount { get; set; }
    }
}