using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PokeApp.Core.Application.Validations;

namespace PokeApp.Core.Application
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddServicesFromApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<PokemonValidator>();
        }
    }
}
