using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Services.UserAccountsService
{
    public interface IUserAccountService
    {
        public Task<User> GetUserByUserName(string username);
        public Task<User> GetUserByCognitoUserId(string cognitoUserId);
        public Task<WeekAndDayResponse> ProgressWeekAndDay(string cognitoUserId);
        public WeekAndDayResponse GetWeekAndDay(string cognitoUserId);
        public Task CreateUser(CreateUser request);

    }

    public class WeekAndDayResponse
    {
        public int Week { get; set; }
        public int Day { get; set; }

    }
}