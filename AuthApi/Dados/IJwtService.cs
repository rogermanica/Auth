using AuthApi.Dominio;
using Microsoft.IdentityModel.JsonWebTokens;

namespace AuthApi.Dados
{
    public interface IJwtService
    {
        Dominio.JsonWebToken CreateJsonWebToken(User user);
    }
}