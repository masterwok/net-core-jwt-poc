using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Config;
using Api.Constants;
using Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        private static readonly User DemoUser = new User
        {
            Id = 1,
            FirstName = "Jonathan",
            LastName = "Trowbridge",
            Username = "masterwok"
        };

        private readonly SigningCredentials _singingCredentials;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;

            _singingCredentials = new SigningCredentials(
                _jwtConfig.SecurityKey
                , SecurityAlgorithms.HmacSha256Signature
            );
        }

        public async Task<AuthResult> Authenticate(string username, string password)
        {
            if (password != "foo")
            {
                return new AuthResult {AuthStatus = AuthStatus.Failure};
            }

            return new AuthResult
            {
                AuthStatus = AuthStatus.Success,
                Token = GenerateToken(DemoUser)
            };
        }

        private string GenerateToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString())
                }),
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = _singingCredentials
            };

            var token = JwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return JwtSecurityTokenHandler.WriteToken(token);
        }
    }
}