using AutoMapper;
using CartService.Application.DTO;
using CartService.Application.Repositories;
using CartService.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Services.Customer
{
    public class CustomerShoppingCartService : ICustomerShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IMapper _mapper;

        public CustomerShoppingCartService(
            IUnitOfWork unitOfWork,
            IShoppingCartRepository shoppingCartRepository,
            IShoppingCartItemRepository shoppingCartItemRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _mapper = mapper;
        }

        public Task<bool> CheckoutShoppingCartAsync(ShoppingCartCheckoutDTO value)
        {
            throw new NotImplementedException();

            // validate shopping cart
            // create Order
            // remove shopping cart
        }

        public async Task<ShoppingCartDTO> GetShoppingCartAsync(string customerId)
        {
            return await _unitOfWork.RunInTrunsaction(async () =>
            {
                var cart = await _shoppingCartRepository.GetByCustomerId(customerId, includeItems: true);

                if (cart != null)
                    return _mapper.Map<ShoppingCart, ShoppingCartDTO>(cart);

                return new ShoppingCartDTO(customerId);
            });
        }

        public async Task AddProductAsync(string customerId, long productId, int quantity)
        {
            await _unitOfWork.RunInTrunsaction(async () =>
            {
                // TODO: get COST from product repository

                var cart = await _shoppingCartRepository.GetByCustomerId(customerId, includeItems: true);
                if (cart == null)
                {
                    cart = new ShoppingCart(customerId);
                    await _shoppingCartRepository.Add(cart);

                    var item = new ShoppingCartItem(cart.Id, productId, 0, quantity);
                    await _shoppingCartItemRepository.Add(item);

                    return;
                }

                cart.LatestUpdatedOn = DateTimeOffset.UtcNow;
                await _shoppingCartRepository.Update(cart);

                var cartItem = cart.ShoppingCartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                {
                    var item = new ShoppingCartItem(cart.Id, productId, 0, quantity);
                    await _shoppingCartItemRepository.Add(item);
                }
                else
                {
                    cartItem.Quantity += quantity;
                    await _shoppingCartItemRepository.Update(cartItem);
                }

            }, System.Data.IsolationLevel.Serializable);
        }

        public async Task DeleteProductAsync(string customerId, long productId, int quantity)
        {
            await _unitOfWork.RunInTrunsaction(async () =>
            {
                var cart = await _shoppingCartRepository.GetByCustomerId(customerId, includeItems: true);
                if (cart == null)
                    return;

                cart.LatestUpdatedOn = DateTimeOffset.UtcNow;
                await _shoppingCartRepository.Update(cart);

                var cartItem = cart.ShoppingCartItems.FirstOrDefault(x => x.ProductId == productId);
                if (cartItem == null)
                    return;

                if (cartItem.Quantity > quantity)
                {
                    cartItem.Quantity -= quantity;
                    await _shoppingCartItemRepository.Update(cartItem);
                }
                else
                {
                    await _shoppingCartItemRepository.Delete(cartItem);
                }
            });
        }

        public async Task DeleteShoppingCartAsync(string customerId)
        {
            await _unitOfWork.RunInTrunsaction(async () =>
            {
                await _shoppingCartRepository.DeleteByCustomerId(customerId);
            });
        }
    }
}
