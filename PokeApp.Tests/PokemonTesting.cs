using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PokeApp.Core.Application.Interfaces.Repositories;
using PokeApp.Core.Application.Interfaces.Services;
using PokeApp.Core.Application.Responses;
using PokeApp.Core.Application.Services;
using PokeApp.Core.Domain.Settings;
using PokeApp.Presentation.API.Controllers;
using Presentation.Infrastructure.Repositories;

namespace PokeApp.Tests
{
    public class PokemonTesting
    {
        private readonly AbilitiesController _abilitiesController;
        private readonly IPokemonService _service;

        public PokemonTesting()
        {
            IPokemonRepository repository = new PokemonRepository();
            IOptions<EndpointSettings> options = Options.Create<EndpointSettings>(new EndpointSettings() { Url = "https://pokeapi.co/api/v2/pokemon" });

            _service = new PokemonService(repository);
            _abilitiesController = new AbilitiesController(_service, options);
        }
        [Fact]
        public async Task Get_Ok()
        {
            var result = await _abilitiesController.Get("pikachu");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Get_QuantityOcultas()
        {
            var result = (ObjectResult)(await _abilitiesController.Get("pikachu"));

            var abilities = Assert.IsType<Result<PokemonResponse>>(result.Value);
            Assert.True(abilities != null || abilities.Data.ocultas.Count > 0);
        }
    }
}