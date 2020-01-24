using System;
using System.Threading.Tasks;

namespace CartService.Application.Services.Background
{
    public interface IBackgroundShoppingCartService
    {
        Task DeleteExpiredShoppingCarts();
        Task GenerateReport();
    }
}
