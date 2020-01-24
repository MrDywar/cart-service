namespace CartService.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool ForBonusPoints { get; set; }
    }
}
