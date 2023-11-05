using OperationStacked.Data;
using Microsoft.EntityFrameworkCore;

namespace OperationStacked.Repositories
{
    public abstract class RepositoryBase
    {
        private IDbContextFactory<OperationStackedContext> _contextFactory { get; }
        protected OperationStackedContext _operationStackedContext => _contextFactory.CreateDbContext();

        protected RepositoryBase(IDbContextFactory<OperationStackedContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
