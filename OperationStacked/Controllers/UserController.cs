using Microsoft.AspNetCore.Mvc;
using OperationStacked.Abstractions;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services;
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
        {
            try
            {
                var _ = await _userAccountService.ProgressWeekAndDay(userid);
                return Ok(_);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [Route("week-and-day/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentWeekAndDay([FromRoute] int userId)
        {
          return Ok(_userAccountService.GetWeekAndDay(userId));
        }
        
    }
}
