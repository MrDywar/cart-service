using CartService.Application.Services;
using CartService.Domain.Entities;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Repositories
{
    public class ShoppingCartRepository : RepositoryBase, IShoppingCartRepository
    {
        public ShoppingCartRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task Add(ShoppingCart entity)
        {
            var sqlQuery = @"INSERT INTO ShoppingCart (CustomerId, CreatedOn, LatestUpdatedOn)
                             VALUES (@CustomerId, @CreatedOn, @LatestUpdatedOn);
                             SELECT SCOPE_IDENTITY()";

            entity.Id = await Connection.ExecuteScalarAsync<long>(sqlQuery, entity, Transaction);
        }

        public async Task Delete(long id)
        {
            var sqlQuery = @"DELETE
                            FROM ShoppingCart
                            WHERE Id = @id";

            await Connection.ExecuteAsync(sqlQuery, new { id }, Transaction);
        }

        public async Task DeleteByCustomerId(string customerId)
        {
            var sqlQuery = @"DELETE
                            FROM ShoppingCart
                            WHERE CustomerId = @customerId";

            await Connection.ExecuteAsync(sqlQuery, new { customerId }, Transaction);
        }

        public async Task<ShoppingCart> GetByCustomerId(string customerId, bool includeItems = false)
        {
            if (!includeItems)
            {
                var sqlQuery = @"SELECT *
                                FROM ShoppingCart
                                WHERE CustomerId = @customerId";

                var result = await Connection.QueryAsync<ShoppingCart>(sqlQuery, new { customerId }, Transaction);

                return result.FirstOrDefault();
            }
            else
            {
                var sqlQuery = @"
                    SELECT *
                    FROM ShoppingCart c
                    LEFT JOIN ShoppingCartItem ci on ci.ShoppingCartId = c.Id
                    WHERE c.CustomerId = @customerId";

                var cartItems = new List<ShoppingCartItem>();

                var carts = await Connection.QueryAsync<ShoppingCart, ShoppingCartItem, ShoppingCart>(
                        sqlQuery, (cart, cartItem) =>
                        {
                            cartItems.Add(cartItem);
                            return cart;
                        },
                        param: new { customerId },
                        transaction: Transaction);

                var result = carts.FirstOrDefault();
                if (result != null)
                {
                    result.ShoppingCartItems = cartItems;
                }

                return result;
            }
        }

        public async Task<ShoppingCart> Get(long Id)
        {
            var sqlQuery = @"SELECT *
                            FROM ShoppingCart
                            WHERE Id = @Id";

            var result = await Connection.QueryAsync<ShoppingCart>(sqlQuery, new { Id }, Transaction);

            return result.FirstOrDefault();
        }

        public async Task Update(ShoppingCart entity)
        {
            var sqlQuery = @"UPDATE ShoppingCart
                             SET LatestUpdatedOn = @LatestUpdatedOn
                             WHERE Id = @Id";

            await Connection.ExecuteAsync(sqlQuery, entity, Transaction);
        }
    }
}
