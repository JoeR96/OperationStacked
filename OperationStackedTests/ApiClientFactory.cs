using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using OperationStacked;
using OperationStacked.TestLib;

namespace OperationStackedAuth.Tests
{
    internal class ApiClientFactory
    {
        private static readonly WebApplicationFactory<Program> Factory = new WebApplicationFactory<Program>();

        internal static async Task<(WorkoutClient, Guid)> CreateWorkoutClientAsync(bool withToken = false)
        {
            var client = Factory.CreateClient();
            var userId = Guid.Empty;

            if (withToken)
            {
                var authClient = CreateAuthClientAsync();
                var authResponse = await authClient.LoginAsync(new LoginRequest
                {
                    Email = "joeyyrichardson96@gmail.com",
                    Password = "Zelfdwnq9512!"
                });
                var token = authResponse.AccessToken;
                userId = authResponse.UserId;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return (new WorkoutClient(client.BaseAddress.AbsoluteUri,client), userId);
        }

        internal static  AuthClient CreateAuthClientAsync()
        {
            var client = new HttpClient();
            const string baseUrl = "http://13.40.62.72:5001";
            return new AuthClient(baseUrl, client);
        }
    }
}
