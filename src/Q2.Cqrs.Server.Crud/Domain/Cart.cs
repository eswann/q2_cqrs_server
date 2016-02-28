using System;
using System.Collections.Generic;

namespace Q2.Cqrs.Server.Crud.Domain
{
    public class Cart
    {
		public Guid Id { get; set; }

	    public IList<CartItem> CartItems { get; } = new List<CartItem>();
    }
}
