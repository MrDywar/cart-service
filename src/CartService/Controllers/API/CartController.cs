using CartService.Application.DTO;
using CartService.Application.Services.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CartService.Controllers.API
{
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICustomerShoppingCartService _shoppingCartService;
        private readonly string _customerId;

        public CartController(ICustomerShoppingCartService shoppingCartService, IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartService = shoppingCartService;
            _customerId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        [Route("api/cart"), HttpGet]
        public async Task<ShoppingCartDTO> Get()
        {
            return await _shoppingCartService.GetShoppingCartAsync(_customerId);
        }

        [Route("api/cart/product"), HttpPost]
        public async Task AddProduct(long productId, int quantity)
        {
            await _shoppingCartService.AddProductAsync(_customerId, productId, quantity);
        }

        [Route("api/cart/product"), HttpDelete]
        public async Task DeleteProduct(long productId, int quantity)
        {
            await _shoppingCartService.DeleteProductAsync(_customerId, productId, quantity);
        }

        [Route("api/cart/checkout"), HttpPost]
        public async Task Checkout([FromBody] ShoppingCartCheckoutDTO value)
        {
            await _shoppingCartService.CheckoutShoppingCartAsync(value);
        }

        [Route("api/cart"), HttpDelete]
        public async Task Delete()
        {
            await _shoppingCartService.DeleteShoppingCartAsync(_customerId);
        }
    }
}
