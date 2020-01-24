using CartService.Application.Services;
using CartService.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Repositories
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<Product> Get(long Id)
        {
            var sqlQuery = @"SELECT *
                            FROM Product
                            WHERE Id = @Id";

            var result = await Connection.QueryAsync<Product>(sqlQuery, new { Id }, Transaction);

            return result.FirstOrDefault();
        }
    }
}
