using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class Person
    {
        public bool Confirmed { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
    }
}