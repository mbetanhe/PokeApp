using PokeApp.Core.Application.Responses;
using PokeApp.Core.Domain.Entities;
using System.Threading.Tasks;

namespace PokeApp.Core.Application.Interfaces.Repositories
{
    public interface IPokemonRepository
    {
        Task<PokemonResponse> GetHiddenAbilitiesAsync(string PokemonName, string endpoint);
    }
}
