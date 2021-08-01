using System;

namespace TradeLib.Models
{
    public class PersonProductsModel
    {
        public Guid Id { get; set; }
        
        public Guid? PersonId { get; set; }
        
        public Guid? ProductId { get; set; }
    }
}