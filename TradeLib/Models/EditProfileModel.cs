using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class EditProfileModel
    {
        [Required(ErrorMessage = "You didn't enter name.")]
        public string Name { get; set; }
    }
}