using CartService.Application.DTO;
using System.Threading.Tasks;

namespace CartService.Application.Services.Customer
{
    public interface ICustomerShoppingCartService
    {
        Task<ShoppingCartDTO> GetShoppingCartAsync(string customerId);
        Task AddProductAsync(string customerId, long productId, int quantity);
        Task DeleteProductAsync(string customerId, long productId, int quantity);
        Task<bool> CheckoutShoppingCartAsync(ShoppingCartCheckoutDTO value);
        Task DeleteShoppingCartAsync(string customerId);
    }
}
