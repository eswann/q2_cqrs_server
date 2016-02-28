using MongoDB.Driver;
using Q2.Cqrs.Server.Crud.Domain;
using System.Threading.Tasks;

namespace Q2.Cqrs.Server.Crud.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly IMongoDatabase _database;

		public CartRepository(IMongoDatabase database)
		{
			_database = database;
		}

		public async Task SaveCartAsync(Cart cart)
		{
			var collection = _database.GetCollection<Cart>("Carts");

			await collection.ReplaceOneAsync(c => c.Id == cart.Id, cart,
				new UpdateOptions { IsUpsert = true});
		}
	}
}