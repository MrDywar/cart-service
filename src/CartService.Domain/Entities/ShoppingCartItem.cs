namespace CartService.Domain.Entities
{
    public class ShoppingCartItem : Entity
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }

        public ShoppingCartItem()
        {

        }

        public ShoppingCartItem(long shoppingCartId, long productId, decimal cost, int quantity)
        {
            ShoppingCartId = shoppingCartId;
            ProductId = productId;
            Cost = cost;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Id: {Id}, ShoppingCartId: {ShoppingCartId}, ProductId: {ProductId}, Cost: {Cost}, Quantity: {Quantity}";
        }
    }
}
