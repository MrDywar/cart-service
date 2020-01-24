using System.Collections.Generic;

namespace CartService.Application.DTO
{
    public class ShoppingCartDTO : EntityDTO
    {
        public string CustomerId { get; set; }
        public List<ShoppingCartItemDTO> ShoppingCartItems { get; set; } = new List<ShoppingCartItemDTO>();

        public ShoppingCartDTO()
        {

        }

        public ShoppingCartDTO(string customerId)
        {
            CustomerId = customerId;
        }
    }
}
