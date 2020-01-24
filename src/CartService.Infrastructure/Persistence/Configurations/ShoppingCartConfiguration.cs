using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Persistence.Configurations
{
    public class ShoppingCartConfiguration : EntityBaseConfiguration<ShoppingCart>
    {
        public override void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasOne(order => order.Customer)
                .WithMany()
                .HasForeignKey(order => order.CustomerId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
