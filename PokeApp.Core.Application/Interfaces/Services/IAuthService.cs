using PokeApp.Core.Application.Requests;
using PokeApp.Core.Application.Responses;
using System.Threading.Tasks;

namespace PokeApp.Core.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> GetTokenAsync(LoginRequest data);
    }
}
