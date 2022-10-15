using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Services;

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

        public async Task<User> GetUserById(int userid)
        => await _context.Users.Where(x => x.UserId == userid)?
        .FirstOrDefaultAsync();

        public async Task<WeekAndDayResponse> ProgressWeekAndDay(int userid)
        {
            var user = await GetUserById(userid);

            var workoutsInWeek = user.WorkoutDaysInWeek;
            var currentDay = user.CurrentDay;
            var currentWeek = user.CurrentWeek;

            if(currentDay < workoutsInWeek)
            {
                user.CurrentDay += 1;

            }
            else if (currentDay == workoutsInWeek)
            {
                user.CurrentDay = 1;
                user.CurrentWeek += 1;
            }
            await _context.SaveChangesAsync();

            return new WeekAndDayResponse
            {
                Day = user.CurrentDay,
                Week = user.CurrentWeek
            };

        }

        public WeekAndDayResponse GetWeekAndDay(int userId) => GetUserById(userId).Result.GetWeekAndDayResponse();

        
        
    }
}
public static class UserExtensions
{
    public static WeekAndDayResponse GetWeekAndDayResponse(this User user) => new WeekAndDayResponse
    {
        Week = user.CurrentWeek,
        Day = user.CurrentDay
    };
}

