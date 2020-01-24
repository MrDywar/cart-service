using CartService.Application.Services;
using System.Data;

namespace CartService.Application.Repositories
{
    public abstract class RepositoryBase
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected IDbTransaction Transaction { get { return UnitOfWork.Transaction; } }
        protected IDbConnection Connection { get { return UnitOfWork.Transaction.Connection; } }

        public RepositoryBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
