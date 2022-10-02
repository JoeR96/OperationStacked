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
        private const string Email = "TITS@TITHOME.COM";
        private const string Password = "LOL!";
        private const string Username = "TITS@TITTIESHOME.COM";

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
            //registerResponse.EnsureSuccessStatusCode();

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
                EmailAddress = "TITS@TITS@HOME.COM",
                Password = "LOL!"
            };
            var response = await _client.PostAsJsonAsync(registerUrl
               , request);

            var secondresponse = await _client.PostAsJsonAsync(registerUrl
               , request);

            // Assert
            _context.Users.Where(x => x.UserName == user).Count().Should().Be(1);

        }
    }
}
