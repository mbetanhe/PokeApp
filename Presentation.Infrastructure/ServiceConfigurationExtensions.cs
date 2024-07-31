using Microsoft.Extensions.DependencyInjection;
using PokeApp.Core.Application.Interfaces.Repositories;
using PokeApp.Core.Application.Interfaces.Services;
using PokeApp.Core.Application.Services;
using PokeApp.Infrastructure.Services;
using Presentation.Infrastructure.Repositories;

namespace PokeApp.Infrastructure
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddServicesFromInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IPokemonRepository, PokemonRepository>();
            services.AddTransient<IPokemonService, PokemonService>();
            services.AddTransient<IAuthService, AuthService>();
        }
    }
}
