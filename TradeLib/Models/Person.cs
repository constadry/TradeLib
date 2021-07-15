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
        [StringLength(64)]
        [EmailAddress(ErrorMessage = "Email must be true")]
        public string Email { get; set; }

        [Required]
        [StringLength(55),MinLength(5, ErrorMessage = "password must be more than 5 chars")]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }
    }
}