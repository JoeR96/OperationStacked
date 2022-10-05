using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;

namespace OperationStacked.Services
{
    public interface IUserAccountService
    {
        public Task<User> GetUserByUserName(string username);

        public Task<User> GetUserByEmail(string email);
    }
}