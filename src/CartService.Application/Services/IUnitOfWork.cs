using System;
using System.Data;
using System.Threading.Tasks;

namespace CartService.Application.Services
{
    public interface IUnitOfWork
    {
        IDbTransaction Transaction { get; }

        Task<TResult> RunInTrunsaction<TResult>(
            Func<Task<TResult>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task RunInTrunsaction(
            Func<Task> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        Task<TResult> RunInTrunsaction<TResult>(
            Func<IDbConnection, IDbTransaction, Task<TResult>> operation,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}
