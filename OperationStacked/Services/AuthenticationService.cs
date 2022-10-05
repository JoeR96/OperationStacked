using OperationStacked.Abstractions;
using OperationStacked.Communication;

namespace OperationStacked.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ITokenHandlerService _tokenHandlerService;

        public AuthenticationService(IUserAccountService userAccountService, ITokenHandlerService tokenService, IPasswordHasherService passwordHasherService)
        {
            _tokenHandlerService = tokenService;
            _passwordHasherService = passwordHasherService;
            _userAccountService = userAccountService;
        }

        public async Task<TokenResponse> CreateAccessTokenAsync(string email, string password)
        {
            var user = await _userAccountService.GetUserByEmail(email);

            if (user == null || !_passwordHasherService.PasswordMatches(password, user.Password))
            {
                return new TokenResponse(false, "Invalid credentials.", null);
            }

            var token = _tokenHandlerService.CreateAccessToken(user);

            return new TokenResponse(true, null, token);
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail)
        {
            var token = _tokenHandlerService.TakeRefreshToken(refreshToken);

            if (token == null)
            {
                return new TokenResponse(false, "Invalid refresh token.", null);
            }

            if (token.IsExpired())
            {
                return new TokenResponse(false, "Expired refresh token.", null);
            }

            var user = await _userAccountService.GetUserByEmail(userEmail);
            if (user == null)
            {
                return new TokenResponse(false, "Invalid refresh token.", null);
            }

            var accessToken = _tokenHandlerService.CreateAccessToken(user);
            return new TokenResponse(true, null, accessToken);
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            _tokenHandlerService.RevokeRefreshToken(refreshToken);
        }
    }
}
