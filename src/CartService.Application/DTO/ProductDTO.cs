namespace CartService.Application.DTO
{
    public class ProductDTO : EntityDTO
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool ForBonusPoints { get; set; }
    }
}
