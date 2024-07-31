using Microsoft.AspNetCore.Mvc;
using PokeApp.Core.Application.Interfaces.Services;
using PokeApp.Core.Application.Requests;
using PokeApp.Core.Application.Responses;
using System.Net;

namespace PokeApp.Presentation.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service) => _service = service;

        [HttpPost("Login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResult<AuthenticationResponse>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(IResult<string>))]
        public async Task<IActionResult> GetTokenAsync(LoginRequest request)
        {
            var result = await _service.GetTokenAsync(request);
            return Ok(result);
        }
    }
}
