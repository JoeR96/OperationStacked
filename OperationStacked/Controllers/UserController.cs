﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Requests;
using OperationStacked.Services.UserAccountsService;
using System.ComponentModel;
using OperationStacked.Response;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Workout Generation")]
    [Route("user/")]
    public class UserController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;

        public UserController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        [Route("updateWeekAndDay")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(WeekAndDayResponse))]
        public async Task<IActionResult> UpdateUserWeekAndDay([FromBody] UpdateWeekAndDayRequest request)
            => Ok(await _userAccountService.UpdateWeekAndDay(request));

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(WeekAndDayResponse))]
        public async Task<IActionResult> UpdateUserWeekAndDay([FromBody] Guid cognitoUserId)
            => Ok(await _userAccountService.ProgressWeekAndDay(cognitoUserId));

        [Route("week-and-day/{userId:guid}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(WeekAndDayResponse))]
        public async Task<IActionResult> GetCurrentWeekAndDay([FromRoute] Guid userId)
            => Ok(await _userAccountService.GetWeekAndDay(userId));

        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task UpdateUser([FromBody] CreateUser request) => await _userAccountService.CreateUser(request);

        [HttpGet("name/{userId:guid}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsername([FromRoute] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest($"Invalid or missing userId. A valid userId is expected. UserId was : {userId}");
            }

            var ua = await _userAccountService.GetUserByUserId(userId);

            if (ua == null || string.IsNullOrEmpty(ua.UserName))
            {
                return Ok(new { UserName = string.Empty });
            }

            return Ok(ua.UserName);
        }


        
        [Route("username/{username}")]
        [HttpGet]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)] // Indicates that this endpoint returns a boolean
        public async Task<IActionResult> GetUserByUserName([FromRoute] string username)
        {
           

            var ua = await _userAccountService.GetUserByUserName(username);
            if (ua == null)
            {
                return Ok(false);
            }
            return Ok(true);
        }

        [Route("set-username")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetUsername([FromBody] SetUsernameRequest request)
        {
            var result = await _userAccountService.SetUsername(request);
            if (result)
            {
                return Ok("Username updated successfully.");
            }

            return BadRequest("Failed to update username.");
        }

    }
}
