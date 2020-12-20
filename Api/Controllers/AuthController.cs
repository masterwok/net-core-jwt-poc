using System;
using System.Threading.Tasks;
using Api.Constants;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(
            IAuthenticationService authenticationService
        ) => _authenticationService = authenticationService;

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthRequest authRequest)
        {
            var result = await _authenticationService.Authenticate(
                authRequest.Username
                , authRequest.Password
            ).ConfigureAwait(false);

            return result.AuthStatus switch
            {
                AuthStatus.Success => Ok(result),
                AuthStatus.Failure => Unauthorized(),
                _ => throw new IndexOutOfRangeException()
            };
        }

        [HttpPost]
        [HttpPost("refresh")]
        public IActionResult Refresh() => Ok("Hey there!");
    }
}