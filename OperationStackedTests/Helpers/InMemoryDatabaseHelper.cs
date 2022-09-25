using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;

namespace OperationStackedTests.Helpers
{
    public class InMemoryDatabaseHelper
    {
        private readonly OperationStackedContext _dataContext;
        public OperationStackedContext DataContext => _dataContext;
        public InMemoryDatabaseHelper()
        {
            var builder = new DbContextOptionsBuilder<OperationStackedContext>();
            builder.UseInMemoryDatabase(databaseName: "OperationStackedTestDB");

            var dbContextOptions = builder.Options;
            _dataContext = new OperationStackedContext(dbContextOptions);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
        }
    }
}
