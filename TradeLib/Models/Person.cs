using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradeLib.Models
{
    public class Person
    {
        [Required][Key]
        public Guid Id { get; set; }
        public bool Confirmed { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
    }
}