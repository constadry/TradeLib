using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class LoginModel 
    {
        [Required(ErrorMessage = "You didn't enter email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You didn't enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}