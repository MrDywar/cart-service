using CartService.Domain.Entities;
using System.Threading.Tasks;

namespace CartService.Application.Repositories
{
    public interface IShoppingCartItemRepository
    {
        Task<ShoppingCartItem> Get(long Id);
        Task<ShoppingCartItem> GetByCartIdAndProductId(long shoppingCartId, long productId);
        Task Add(ShoppingCartItem entity);
        Task Delete(ShoppingCartItem entity);
        Task Update(ShoppingCartItem entity);
    }
}
