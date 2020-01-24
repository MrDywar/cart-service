using CartService.Domain.Entities;
using System.Threading.Tasks;

namespace CartService.Application.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> Get(long Id);
        Task<ShoppingCart> GetByCustomerId(string customerId, bool includeItems = false);
        Task Add(ShoppingCart entity);
        Task Delete(long Id);
        Task DeleteByCustomerId(string customerId);
        Task Update(ShoppingCart entity);
    }
}
