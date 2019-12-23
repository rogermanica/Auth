using AuthApi.Dominio;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace AuthApi.Dados
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(JwtSettings settings)
        {
            _settings = settings;
        }

        public Dominio.JsonWebToken CreateJsonWebToken(User user)
        {
            var identity = GetClaimsIdentity(user);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                IssuedAt = _settings.IssuedAt,
                NotBefore = _settings.NotBefore,
                Expires = _settings.AccessTokenExpiration,
                SigningCredentials = _settings.SigningCredentials
            });

            var accessToken = handler.WriteToken(securityToken);

            return new Dominio.JsonWebToken
            {
                AccessToken = accessToken,
                ExpiresIn = (long)TimeSpan.FromMinutes(_settings.ValidForMinutes).TotalSeconds
            };
        }

        private static ClaimsIdentity GetClaimsIdentity(User user)
        {
            var identity = new ClaimsIdentity
            (
                new GenericIdentity(user.Email),
                new[] {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Name)
                }
            );

            return identity;
        }
    }
}
