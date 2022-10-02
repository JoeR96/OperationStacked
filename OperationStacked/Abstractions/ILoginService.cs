using FluentResult;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Abstractions
{
    public interface ILoginService
    {
        public Result<LoginRequestResult> AttemptLogin(LoginRequest request);
        public Task Logout();
        public Task<Result<RegisterRequestResult>> Register(RegistrationRequest request);
    }
}