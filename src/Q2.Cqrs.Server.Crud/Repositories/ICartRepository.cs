using Q2.Cqrs.Server.Crud.Domain;
using System.Threading.Tasks;

namespace Q2.Cqrs.Server.Crud.Repositories
{
	public interface ICartRepository
	{
		Task SaveCartAsync(Cart cart);
	}
}