using OperationStacked.Communication;

namespace OperationStacked.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        public Task<TokenResponse> CreateAccessTokenAsync(string email, string password);

        public Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail);

        public void RevokeRefreshToken(string refreshToken);
    }
}