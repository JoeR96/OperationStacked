using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Response;
using System.ComponentModel;
using FluentResult;
using OperationStacked.Abstractions;
using OperationStacked.Requests;
using Microsoft.AspNetCore.Authentication;
using OperationStacked.Communication;

namespace OperationStacked.Controllers
{
    [ApiController]
    [DisplayName("Authentication")]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private ILoginService _loginService;
        private IAuthenticationService _authenticationService;
        private ITokenHandlerService _tokenHandlerService;
        public AuthenticationController(ILoginService loginService,IAuthenticationService authenticationService)
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

        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(LoginRequestResult))]
        [ProducesResponseType(400, Type = typeof(LoginRequestResult))]
        public async Task<IActionResult> Login( [FromBody] LoginRequest request) => _loginService.AttemptLogin(request)
           .ToActionResult(this);
      
    }
}
