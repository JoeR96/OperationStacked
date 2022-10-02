using FluentResult;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services
{
    public class LoginService : ILoginService
    {
        private readonly IPasswordHasherService _passwordHasherService;
        private OperationStackedContext _context;

        public LoginService(OperationStackedContext context, IPasswordHasherService passwordHasherService)
        {
            _passwordHasherService = passwordHasherService;
            _context = context;
        }

        public Result<LoginRequestResult> AttemptLogin(LoginRequest request)
        {
            var user = request.UserName == string.Empty ? 
                GetUserByEmail(request.EmailAddress).Result : GetUserByUserName(request.UserName).Result;

            return _passwordHasherService.PasswordMatches(request.Password, user.Password) ? Login(request) : Result.Create(new LoginRequestResult(false, "Invalid Password;"));
        }

        private Result<LoginRequestResult> Login(LoginRequest request)
        {
            return Result.Create(new LoginRequestResult(true, "Success","JWT"));
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<RegisterRequestResult>> Register(RegistrationRequest request)
        {
            var user = await GetUserByEmail(request.EmailAddress);

            if (user != null)
                return Result.Create(new RegisterRequestResult(false, "User with this email already exists"));

            User userEntity = new User
            {
                UserName = request.UserName,
                Password = _passwordHasherService.HashPassword(request.Password),
                Email = request.EmailAddress
            };

            _context.Users.Add(userEntity);
            _context.SaveChanges();
            return Result.Create(new RegisterRequestResult(true, ""));
        }

        private async Task<User> GetUserByUserName(string username)
            => await _context.Users.Where(x => x.UserName == username)
            .FirstOrDefaultAsync();

            private async Task<User> GetUserByEmail(string email)
            => await _context.Users.Where(x => x.Email == email)
            .FirstOrDefaultAsync();


    }
}
