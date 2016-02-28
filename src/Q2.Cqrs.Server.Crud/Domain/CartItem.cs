using System;

namespace Q2.Cqrs.Server.Crud.Domain
{
    public class CartItem
    {
		public Guid ProductId { get; set; }

		public Guid Quantity { get; set; }

		public decimal Subtotal { get; set; }
    }
}
