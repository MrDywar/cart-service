using CartService.Application.Services;
using CartService.Domain.Entities;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace CartService.Application.Repositories
{
    public class ShoppingCartItemRepository : RepositoryBase, IShoppingCartItemRepository
    {
        public ShoppingCartItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task Add(ShoppingCartItem entity)
        {
            var sqlQuery = @"INSERT INTO ShoppingCartItem (ProductId, ShoppingCartId, Cost, Quantity)
                             VALUES(@ProductId, @ShoppingCartId, @Cost, @Quantity)";

            await Connection.ExecuteAsync(sqlQuery, entity, Transaction);
        }

        public async Task Delete(ShoppingCartItem entity)
        {
            var sqlQuery = @"DELETE
                            FROM ShoppingCartItem
                            WHERE Id = @Id";

            await Connection.ExecuteAsync(sqlQuery, new { entity.Id }, Transaction);
        }

        public async Task<ShoppingCartItem> Get(long Id)
        {
            var sqlQuery = @"SELECT *
                            FROM ShoppingCartItem
                            WHERE Id = @Id";

            var result = await Connection.QueryAsync<ShoppingCartItem>(sqlQuery, new { Id }, Transaction);

            return result.FirstOrDefault();
        }

        public async Task<ShoppingCartItem> GetByCartIdAndProductId(long shoppingCartId, long productId)
        {
            var sqlQuery = @"SELECT *
                            FROM ShoppingCartItem
                            WHERE ShoppingCartId = @shoppingCartId
                                AND
                                ProductId = @productId";

            var result = await Connection.QueryAsync<ShoppingCartItem>(sqlQuery, new { shoppingCartId, productId }, Transaction);

            return result.FirstOrDefault();
        }

        public async Task Update(ShoppingCartItem entity)
        {
            var sqlQuery = @"UPDATE ShoppingCartItem
                             SET ProductId = @ProductId,
                                 ShoppingCartId = @ShoppingCartId,
                                 Cost = @Cost,
                                 Quantity = @Quantity
                             WHERE Id = @Id";

            await Connection.ExecuteAsync(sqlQuery, entity, Transaction);
        }
    }
}
