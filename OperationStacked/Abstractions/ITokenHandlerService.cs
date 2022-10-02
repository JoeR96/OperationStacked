using OperationStacked.Communication;

namespace OperationStacked.Abstractions
{
    public interface ITokenHandlerService
    {

        public RefreshToken TakeRefreshToken(string token);

        public void RevokeRefreshToken(string token);

    }
}