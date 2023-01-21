using FluentAssertions;
using NUnit.Framework;
using OperationStacked.Requests;
using System.Net.Http.Json;

namespace OperationStackedTests.Functional
{
    [TestFixture]
    public class LoginTests : BaseApiTest
    {
        private const string registerUrl = "/auth/register";
        private const string loginUrl = "/auth/login";
        private const string Email = "email@email.COM";
        private const string Password = "LOL!";
        private const string Username = "email@email.COM";

        [Test]
        public async Task RegistrationCompletes()
        {

            // Act
            var registerRequest = new RegistrationRequest()
            {
                UserName = Username,
                EmailAddress = Email,
                Password = Password
            };

             var registerResponse = await _client.PostAsJsonAsync(registerUrl
               , registerRequest);

            // Assert
            registerResponse.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task RegistrationCompletesAndLogsIn()
        {
            // Act
            var registerRequest = new RegistrationRequest()
            {
                UserName = Username,
                EmailAddress = Email,
                Password = Password
            };
            var registerResponse = await _client.PostAsJsonAsync(registerUrl
              , registerRequest);

            // Assert
            LoginRequest loginRequest = new()
            {
                Email = Email,
                Username = Username,
                Password = Password
            };
            var loginresponse = await _client.PostAsJsonAsync(loginUrl, registerRequest);

            loginresponse.EnsureSuccessStatusCode();

        }

        [Test]
        public async Task RegistrationFailsWithDuplicate()
        {

            // Act
            const string user = "UniqueUser";
            var request = new RegistrationRequest()
            {
                UserName = user,
                EmailAddress = "email@email@HOME.COM",
                Password = "LOL!"
            };
            await _client.PostAsJsonAsync(registerUrl
               , request);

            await _client.PostAsJsonAsync(registerUrl
               , request);

            // Assert
            _context.Users.Where(x => x.UserName == user).Count().Should().Be(1);

        }
    }
}
