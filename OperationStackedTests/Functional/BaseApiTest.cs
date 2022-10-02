using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OperationStacked.Data;
using OperationStackedTests.Helpers;
namespace OperationStackedTests.Functional
{
    public class BaseApiTest
    {
        protected WebApplicationFactory<Program> _application;
        protected HttpClient _client;
        protected OperationStackedContext _context;

        [OneTimeSetUp]
        protected void SetUp()
        {
            _application = Setup();
            _context = new InMemoryDatabaseHelper().DataContext;
            _client = _application.CreateClient();
        }

        private static WebApplicationFactory<Program> Setup()
        {
            var application = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<OperationStackedContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }


                // Add a database context using an in-memory 
                // database for testing.
                services.AddDbContext<OperationStackedContext>(options =>
                {
                    options.UseInMemoryDatabase("OperationStackedTestDB");
                });
            }
            );
        });
            return application;
        }
    }
}