# cart-service

Given:
Product model
```
class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public bool ForBonusPoints { get; set; }
}
```

Task:
Implement CartService

Requirements:
1. Stack: Asp Net Core 3.1 WEB Api + storage of choice Redis / Sql Server + Dapper

2. Functionality:
- Add / remove an arbitrary number of products
- Store basket data for 30 days
- The ability to register web hooks that need to be pulled when the basket is deleted after the expiration of the storage period
- once a day generate and save a report (txt / pdf / excel to choose from), which will indicate:
  - how many baskets
  - how many of them contain products for points
  - how many baskets expire within 10/20/30 days
  - average check basket

Notes:
- no validation
- no tests
- no additional indexes on DB (LatestUpdatedOn for search, ProductId+ShoppingCartId for unique constraint)