using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class ToRestoreModel
    {
        [Required(ErrorMessage = "You didn't enter email.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}