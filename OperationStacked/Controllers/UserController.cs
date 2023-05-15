using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Requests;
using OperationStacked.Services.UserAccountsService;
using System.ComponentModel;

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

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserWeekAndDay([FromBody] UserIdRequest request)
            => Ok(await _userAccountService.ProgressWeekAndDay(request.CognitoUserId));

        [Route("week-and-day/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeekAndDay([FromRoute] string userId)
            => Ok(_userAccountService.GetWeekAndDay(userId));

        [HttpPost("update-create-user")]
        [AllowAnonymous]
        public async Task UpdateUser([FromBody] CreateUser request) => await _userAccountService.CreateUser(request);
        [Route("name")]
        [HttpGet]
        public async Task<IActionResult> GetUsername(string cognitoUserId)
        {
            var ua = await _userAccountService.GetUserByCognitoUserId(cognitoUserId);
            var t = ua.UserName;
            return Ok(t);
        }
    }
}