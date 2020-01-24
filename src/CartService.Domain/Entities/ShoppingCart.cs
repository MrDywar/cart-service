using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CartService.Domain.Entities
{
    public class ShoppingCart : Entity
    {
        public ShoppingCart()
        {

        }

        public ShoppingCart(string customerId)
        {
            CustomerId = customerId;
        }

        public string CustomerId { get; set; }
        public IdentityUser Customer { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LatestUpdatedOn { get; set; } = DateTimeOffset.UtcNow;
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

        public override string ToString()
        {
            return $"Id: {Id}, CustomerId: {CustomerId}, CreatedOn: {CreatedOn}, LatestUpdatedOn: {LatestUpdatedOn}, ItemsCount: {ShoppingCartItems.Count}";
        }
    }
}
