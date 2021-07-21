using System;

namespace TradeLib.Models
{
    public class CartPositions
    {
        public Guid Id { get; set; }
        public Guid? PersonId { get; init; }
        public Guid? ProductId { get; init; }
    }
}