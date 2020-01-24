using System.Data;

namespace CartService.Application.Services
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
