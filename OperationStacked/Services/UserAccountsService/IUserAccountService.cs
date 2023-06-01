using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.UserAccountsService
{
    public interface IUserAccountService
    {
        public Task<User> GetUserByUserName(string username);
        public Task<User> GetUserByCognitoUserId(Guid cognitoUserId);
        public Task<WeekAndDayResponse> ProgressWeekAndDay(Guid cognitoUserId);
        public WeekAndDayResponse GetWeekAndDay(Guid cognitoUserId);
        public Task CreateUser(CreateUser request);

    }
    
}