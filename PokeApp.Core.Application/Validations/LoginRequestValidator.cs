using FluentValidation;
using PokeApp.Core.Application.Requests;

namespace PokeApp.Core.Application.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotNull();
        }
    }
}
