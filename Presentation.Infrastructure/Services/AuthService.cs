using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PokeApp.Core.Application.Interfaces.Services;
using PokeApp.Core.Application.Requests;
using PokeApp.Core.Application.Responses;
using PokeApp.Core.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PokeApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly JWTSettings _jwt;

        public AuthService(IOptions<JWTSettings> jwt) => _jwt = jwt.Value;

        private bool CheckBasicUser(LoginRequest request)
        {
            if (request == null) return false;

            if (request.Email == "admin@admin.com" && request.Password == "Admin123.*")
                return true;
            else
                return false;
        }
        public async Task<AuthenticationResponse> GetTokenAsync(LoginRequest data)
        {
            AuthenticationResponse authenticationResponse = new AuthenticationResponse();

            if (data == null)
            {
                authenticationResponse.IsAuthenticated = false;
                authenticationResponse.Message = $"No hay información de autenticacion.";
                // return Result<AuthenticationResponse>.Success(authenticationResponse);
                return authenticationResponse;

            }
            if (CheckBasicUser(data))
            {
                authenticationResponse.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(data);
                authenticationResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationResponse.Id = new Guid().ToString();
                authenticationResponse.UserName = data.Email;
                return authenticationResponse;
            }
            authenticationResponse.IsAuthenticated = false;
            authenticationResponse.Message = $"Credenciales incorrectas.";
            //return Result<AuthenticationResponse>.Success(authenticationResponse);

            return authenticationResponse;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(LoginRequest user)
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", new Guid().ToString())
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
