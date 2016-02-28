using System;

namespace Q2.Cqrs.Server.Crud.Domain
{
	public class Product
	{
		public Guid Id { get; set; }
		
		public string Name { get; set; }
		
		public string Description { get; set; } 

		public string Image { get; set; }

		public decimal Price { get; set; }
	}
}