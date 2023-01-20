using FluentAssertions;
using FluentResult;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using OperationStacked.Abstractions;
using OperationStacked.Entities;
using OperationStacked.Services.AuthenticationService;
using OperationStacked.Services.UserAccountsService;

namespace OperationStackedTests.UnitTests
{
    [TestFixture]
    internal class AuthenticationServiceTests
    {
        IUserAccountService _userAccountService = Substitute.For<IUserAccountService>();
        ITokenHandlerService _tokenHandlerService = Substitute.For<ITokenHandlerService>();
        IPasswordHasherService _passwordHasherService = Substitute.For<IPasswordHasherService>();
        IAuthenticationService authenticationService;

        public AuthenticationServiceTests()
        {
            authenticationService = new AuthenticationService(_userAccountService, _tokenHandlerService, _passwordHasherService);
        }

        [Test]
        public async Task ErrorWhenUserIsNull()
        {
            var _ = _userAccountService.GetUserByEmail(String.Empty).ReturnsNull();
                              
            var result = await authenticationService.CreateAccessTokenAsync(string.Empty, string.Empty);
            
            result.Token.Should().BeNull();
            result.Success.Should().BeFalse();
        }

        [Test]
        public async Task ErrorWhenPasswordsDontMatch()
        {
            var _ = _userAccountService.GetUserByEmail(String.Empty).Returns(new User
            {
                Email = "Jeff",
                Password = "Jeff"
            });

            var result = await authenticationService.CreateAccessTokenAsync("Jeff", "NotThePW");

            result.Token.Should().BeNull();
            result.Success.Should().BeFalse();
        }
    }
}
