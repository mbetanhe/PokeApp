using PokeApp.Core.Application.Responses;
using System.Threading.Tasks;

namespace PokeApp.Core.Application.Interfaces.Services
{
    public interface IPokemonService
    {
        Task<IResult> GetHiddenAbilitiesAsync(string PokemonName, string endpoint);
    }
}
