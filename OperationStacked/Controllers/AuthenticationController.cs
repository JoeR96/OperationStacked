using System.ComponentModel;
using FluentResult;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Abstractions;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Authentication")]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private IAuthenticationService _authenticationService;
        private ITokenHandlerService _tokenHandlerService;
        public AuthenticationController(ILoginService loginService,
            IAuthenticationService authenticationService)
        {
            _loginService = loginService;
            _authenticationService = authenticationService;
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(RegisterRequestResult))]
        [ProducesResponseType(400, Type = typeof(RegisterRequestResult))]
        public async Task<IActionResult> Register( [FromBody] RegistrationRequest request) => await _loginService.Register(request)
            .ToActionResultAsync(this);

        [Route("dummyRegister")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(RegisterRequestResult))]
        [ProducesResponseType(400, Type = typeof(RegisterRequestResult))]
        public async Task<IActionResult> DummyRegister()
        {
            var request = new RegistrationRequest
            {
                UserName = "BigDaveTv",
                EmailAddress = "BigDaveTV@gmail.com",
                Password = "BigDaveTV"
            }; 
             return await _loginService.Register(request)
            .ToActionResultAsync(this);
        }

        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(LoginRequestResult))]
        [ProducesResponseType(400, Type = typeof(LoginRequestResult))]
        public async Task<IActionResult> Login( [FromBody] LoginRequest request) => _loginService.AttemptLogin(request)
           .ToActionResult(this);
      
    }
}
