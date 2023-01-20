using FluentResult;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Abstractions;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.UserAccountsService;

namespace OperationStacked.Services.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IPasswordHasherService _passwordHasherService;
        private OperationStackedContext _context;
        private ITokenHandlerService _tokenHandlerService;
        private readonly IUserAccountService _userAccountService;
        public LoginService(OperationStackedContext context, IPasswordHasherService passwordHasherService, ITokenHandlerService tokenHandlerService, IUserAccountService userAccountService)
        {
            _passwordHasherService = passwordHasherService;
            _context = context;
            _tokenHandlerService = tokenHandlerService;
            _userAccountService = userAccountService;
        }

        public Result<LoginRequestResult> AttemptLogin(LoginRequest request)
        {
            var user = request.UserName == string.Empty ? _userAccountService.GetUserByEmail(request.EmailAddress).Result :
            _userAccountService.GetUserByUserName(request.UserName).Result;

            return _passwordHasherService.PasswordMatches(request.Password, user.Password) ? Login(user) : Result.Create(new LoginRequestResult(false, "Invalid Password;"));
        }

        private Result<LoginRequestResult> Login(User user)
        {
            return Result.Create(new LoginRequestResult(true, "Success", "JWT", user.UserId, user.CurrentDay, user.CurrentWeek, user.UserName));
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<RegisterRequestResult>> Register(RegistrationRequest request)
        {
            var user = await _userAccountService.GetUserByEmail(request.EmailAddress);

            if (user != null)
                return Result.Create(new RegisterRequestResult(false, "User with this email already exists"));

            User userEntity = new User
            {
                UserName = request.UserName,
                Password = _passwordHasherService.HashPassword(request.Password),
                Email = request.EmailAddress,
                WorkoutDaysInWeek = 4,
                CurrentWeek = 1,
                CurrentDay = 1
            };

            _context.Users.Add(userEntity);
            _context.SaveChanges();
            return Result.Create(new RegisterRequestResult(true, "", userEntity.UserId));
        }
    }
}
