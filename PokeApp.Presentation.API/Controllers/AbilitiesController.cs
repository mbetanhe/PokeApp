using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PokeApp.Core.Application.Interfaces.Services;
using PokeApp.Core.Application.Requests;
using PokeApp.Core.Application.Responses;
using PokeApp.Core.Application.Services;
using PokeApp.Core.Domain.Settings;
using PokeApp.Presentation.API.Services;
using System.Net;

namespace PokeApp.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AbilitiesController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private EndpointSettings _endpoint;

        public AbilitiesController(IPokemonService service, IOptions<EndpointSettings> options) => (_pokemonService, _endpoint) = (service, options.Value);
        //public AbilitiesController(IPokemonService service) => (_pokemonService) = (service);

        [HttpGet("{Pokemon}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResult<IEnumerable<PokemonResponse>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IResult<IEnumerable<PokemonResponse>>))]
        public async Task<IActionResult> Get(string Pokemon)
        {
            var data = SanitizeInputService.SanitizeInput(Pokemon);
            var restul = await _pokemonService.GetHiddenAbilitiesAsync(Pokemon, _endpoint.Url);

            if (!restul.Succeeded)
                BadRequest(restul);

            return Ok(restul);
        }
    }
}
