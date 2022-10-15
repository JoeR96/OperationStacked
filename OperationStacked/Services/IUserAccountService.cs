using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Services
{
    public interface IUserAccountService
    {
        public Task<User> GetUserByUserName(string username);

        public Task<User> GetUserByEmail(string email);
        public Task<WeekAndDayResponse> ProgressWeekAndDay(int userid);
        public WeekAndDayResponse GetWeekAndDay(int userId);
    }

    public class WeekAndDayResponse
    {
        public int Week { get; set; }
        public int Day { get; set; }

    }
}