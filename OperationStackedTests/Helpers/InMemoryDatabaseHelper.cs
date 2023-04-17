using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            var options = Options.Create(new ConnectionStringOptions { ConnectionString = "your_connection_string_here" });

            _dataContext = new OperationStackedContext(dbContextOptions, options);
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
        }
    }
}
