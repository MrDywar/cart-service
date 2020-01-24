using CartService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartService.Infrastructure.Persistence.Configurations
{
    public class ShoppingCartItemConfiguration : EntityBaseConfiguration<ShoppingCartItem>
    {
        public override void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {
            builder.Property(x => x.Cost).HasColumnType("decimal(18, 4)");

            builder.HasOne(x => x.ShoppingCart)
                .WithMany(x => x.ShoppingCartItems)
                .HasForeignKey(x => x.ShoppingCartId)
                .IsRequired();

            builder.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
