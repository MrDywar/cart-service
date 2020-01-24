using CartService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Repositories
{
    public interface IProductRepository
    {
        Task<Product> Get(long Id);
    }
}
