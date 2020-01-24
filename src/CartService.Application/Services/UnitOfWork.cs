using System;
using System.Data;
using System.Threading.Tasks;

namespace CartService.Application.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private IConnectionFactory _connectionFactory;
        private IDbTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbTransaction Transaction { get { return _transaction; } }

        public async Task<TResult> RunInTrunsaction<TResult>(
            Func<Task<TResult>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                using (_transaction = connection.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        var result = await operation();
                        _transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        _transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<TResult> RunInTrunsaction<TResult>(
            Func<IDbConnection, IDbTransaction, Task<TResult>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                using (_transaction = connection.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        var result = await operation(connection, _transaction);
                        _transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        _transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task RunInTrunsaction(
            Func<Task> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                using (_transaction = connection.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        await operation();
                        _transaction.Commit();
                    }
                    catch
                    {
                        _transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public TResult RunInTrunsaction<TResult>(
            Func<TResult> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                using (_transaction = connection.BeginTransaction(isolationLevel))
                {
                    try
                    {
                        var result = operation();
                        _transaction.Commit();
                        return result;
                    }
                    catch
                    {
                        _transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
