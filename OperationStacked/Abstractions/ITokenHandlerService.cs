using OperationStacked.Communication;
using OperationStacked.Entities;

namespace OperationStacked.Abstractions
{
    public interface ITokenHandlerService
    {
        public AccessToken CreateAccessToken(User userAccount);

        public RefreshToken TakeRefreshToken(string token);

        public void RevokeRefreshToken(string token);

    }
}