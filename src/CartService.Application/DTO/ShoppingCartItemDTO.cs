namespace CartService.Application.DTO
{
    public class ShoppingCartItemDTO : EntityDTO
    {
        public long ProductId { get; set; }
        public long ShoppingCartId { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
    }
}
