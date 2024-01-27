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
        [Route("name")]
        [HttpGet]
        [ProducesResponseType(typeof(string),StatusCodes.Status200OK),]
        public async Task<IActionResult> GetUsername(Guid cognitoUserId)
        {
            var ua = await _userAccountService.GetUserByCognitoUserId(cognitoUserId);
            var t = ua.UserName;
            return Ok(t);
        }
    }
}
