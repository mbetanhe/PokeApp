using FluentValidation;
using PokeApp.Core.Application.Requests;

namespace PokeApp.Core.Application.Validations
{
    public class PokemonValidator : AbstractValidator<PokemonRequet>
    {
        public PokemonValidator()
        {
            RuleFor(x => x.Pokemon).NotNull().MaximumLength(20);
        }
    }
}
