﻿using Microsoft.EntityFrameworkCore;
using OperationStacked.Data;
using OperationStacked.Entities;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Services.UserAccountsService
{
    public class UserAccountService : IUserAccountService
    {
        private readonly OperationStackedContext _context;
        public UserAccountService(OperationStackedContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByUserName(string username)
            => await _context.Users.Where(x => x.UserName.ToLower() == username.ToLower())?
            .FirstOrDefaultAsync();

        public async Task<User> GetUserByUserId(Guid userId) => await _context.Users.Where(x => x.Id == userId)?
                .FirstOrDefaultAsync();

        public async Task<User> GetUserById(Guid userId)
        => await _context.Users?.Where(x => x.CognitoUserId == userId)?
        .FirstOrDefaultAsync();

        public async Task<WeekAndDayResponse> ProgressWeekAndDay(Guid userid)
        {
            var user = await GetUserById(userid);

            var workoutsInWeek = user.WorkoutDaysInWeek;
            var currentDay = user.CurrentDay;
            var currentWeek = user.CurrentWeek;

            if (currentDay < workoutsInWeek)
            {
                user.CurrentDay += 1;

            }
            else if (currentDay == workoutsInWeek)
            {
                user.CurrentDay = 1;
                user.CurrentWeek += 1;
            }
            await _context.SaveChangesAsync();

            return new WeekAndDayResponse(
                user.CurrentWeek, user.CurrentDay,user.WorkoutDaysInWeek);

        }

        public async Task<WeekAndDayResponse> GetWeekAndDay(Guid cognitoUserId)
        {
            Guid.Parse(cognitoUserId.ToString());
            var _ = await GetUserById(cognitoUserId);
            return _.GetWeekAndDayResponse();
        }

        public async Task CreateUser(CreateUser request)
        {
            // Check if a user with the same CognitoUserId or UserName already exists
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.CognitoUserId == request.CognitoUserId);
            var existingUserName = await _context.Users.SingleOrDefaultAsync(u => u.UserName == request.UserName);

            if (existingUser != null || existingUserName != null)
            {
                // Gracefully fail - you can log an error message or throw a custom exception here, depending on your application's requirements
                // For example, throw new UserAlreadyExistsException("A user with the same CognitoUserId or UserName already exists.");
                return;
            }

            // If no existing user is found, create a new one
            var newUser = new User
            {
                CognitoUserId = request.CognitoUserId,
                UserName = request.UserName,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task<WeekAndDayResponse> UpdateWeekAndDay(UpdateWeekAndDayRequest request)
        {
            var user = await GetUserById(request.UserId);

                user.CurrentDay = 1;
                user.CurrentWeek = 1;
            await _context.SaveChangesAsync();

            return new WeekAndDayResponse(
                user.CurrentWeek, user.CurrentDay,user.WorkoutDaysInWeek);
        }

        public async Task<bool> SetUsername(SetUsernameRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return false; // User not found
            }

            // Optionally, you might want to check if the username is already taken.
            var isUsernameTaken = await _context.Users.AnyAsync(u => u.UserName == request.Username);
            if (isUsernameTaken)
            {
                return false; // Username is already taken
            }

            user.UserName = request.Username;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
public static class UserExtensions
{
    public static WeekAndDayResponse GetWeekAndDayResponse(this User user) =>
        new (user.CurrentWeek, user.CurrentDay, user.WorkoutDaysInWeek);
}
