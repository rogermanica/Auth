using AuthApi.Dados;
using AuthApi.Dominio;
using AuthApi.Requests;
using AuthApi.Util;
using Flunt.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Handler
{
    public class AuthenticateHandler : IRequestHandler<Authenticate, Response>
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;

        private Response _response;

        public AuthenticateHandler(
            IJwtService jwtService,
            IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<Response> Handle(Authenticate request, CancellationToken cancellationToken)
        {
            _response = new Response();

            User user = user = await HandleUserAuthentication(request);

            if (_response.HasMessages || user == null)
            {
                return _response;
            }

            var jwt = _jwtService.CreateJsonWebToken(user);

            _response.AddValue(new
            {
                access_token = jwt.AccessToken,
                token_type = jwt.TokenType,
                expires_in = jwt.ExpiresIn
            });

            return _response;
        }

        private async Task<User> HandleUserAuthentication(Authenticate request)
        {
            var encodedPassword = new Password(request.Password).Encoded;
            var user = await _userRepository.AuthenticateAsync(request.Email, encodedPassword);

            if (user == null)
            {
                _response.AddNotification(new Notification("user", "Usuário ou senha inválidos"));
            }

            return user;
        }
    }
}