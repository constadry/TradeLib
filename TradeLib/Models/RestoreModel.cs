using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class RestoreModel
    {
        [Required(ErrorMessage = "You didn't enter email.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "You didn't enter password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords aren't equal.")]
        public string RepeatingPassword { get; set; }
    }
}