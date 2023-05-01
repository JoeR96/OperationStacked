using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Requests;
using OperationStacked.Services.UserAccountsService;
using System.ComponentModel;

namespace OperationStacked.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> UpdateUserWeekAndDay([FromBody] string cognitoUserId)
        => Ok(await _userAccountService.ProgressWeekAndDay(cognitoUserId));


        [Route("week-and-day/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeekAndDay([FromRoute] string userId)
            => Ok(_userAccountService.GetWeekAndDay(userId));

        [HttpPost("update-create-user")]
        [AllowAnonymous]
        public async Task UpdateUser([FromBody] CreateUser request) => await _userAccountService.CreateUser(request);
    }
}
