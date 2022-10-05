using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;

namespace OperationStacked.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly OperationStackedContext _context;
        public UserAccountService(OperationStackedContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUserName(string username)
            => await _context.Users.Where(x => x.UserName == username)?
            .FirstOrDefaultAsync();

        public async Task<User> GetUserByEmail(string email)
        => await _context.Users.Where(x => x.Email == email)?
        .FirstOrDefaultAsync();
    }
}
