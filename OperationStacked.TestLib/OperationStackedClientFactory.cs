using Microsoft.AspNetCore.Mvc.Testing;

namespace OperationStacked.TestLib
{
    internal class OperationStackedClientFactory
    {
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
