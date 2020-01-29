using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;

namespace CartService.Application.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        private IDbTransaction _transaction;

        public UnitOfWork(IConnectionFactory connectionFactory, ILogger<UnitOfWork> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public IDbTransaction Transaction { get { return _transaction; } }

        public async Task<TResult> RunInTrunsaction<TResult>(
            Func<Task<TResult>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return await RunInTrunsaction(async (con, tran) =>
            {
                return await operation();
            }, isolationLevel: isolationLevel);
        }

        public async Task RunInTrunsaction(
            Func<Task> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            await RunInTrunsaction(async (con, tran) =>
            {
                await operation();

                return Task.CompletedTask;
            }, isolationLevel: isolationLevel);
        }

        public async Task<TResult> RunInTrunsaction<TResult>(
            Func<IDbConnection, IDbTransaction, Task<TResult>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using var connection = _connectionFactory.GetConnection();
            using (_transaction = connection.BeginTransaction(isolationLevel))
            {
                try
                {
                    var result = await operation(connection, _transaction);
                    _transaction.Commit();

                    return result;
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
                    _logger.LogError(e.Message);

                    throw;
                }
            }
        }
    }
}
