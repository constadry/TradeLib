using System.ComponentModel.DataAnnotations;

namespace TradeLib.Models
{
    public class SearchModel
    {
        [Required(ErrorMessage = "You didn't enter anything.")]
        public string SearchInput { get; set; }
    }
}