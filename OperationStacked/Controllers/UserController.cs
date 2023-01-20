using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> UpdateUserWeekAndDay([FromBody]int userid)
        => Ok(await _userAccountService.ProgressWeekAndDay(userid));


        [Route("week-and-day/{userId:int}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeekAndDay([FromRoute] int userId)
            => Ok(_userAccountService.GetWeekAndDay(userId));
        
        
    }
}
