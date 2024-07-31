using PokeApp.Core.Application.Interfaces.Repositories;
using PokeApp.Core.Application.Interfaces.Services;
using PokeApp.Core.Application.Responses;
using PokeApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace PokeApp.Core.Application.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _repository;

        public PokemonService(IPokemonRepository repository) => _repository = repository;

        public async Task<IResult> GetHiddenAbilitiesAsync(string PokemonName, string endpoint)
        {
            var result = await _repository.GetHiddenAbilitiesAsync(PokemonName, endpoint);

            if (result == null)
                return Result.Fail("No se pudo obtener la información");

            return Result<PokemonResponse>.Success(result);
        }
    }
}
