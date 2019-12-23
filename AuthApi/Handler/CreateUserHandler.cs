using AuthApi.Dados;
using AuthApi.Dominio;
using AuthApi.Requests;
using AuthApi.Util;
using Flunt.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Handler
{
    public class CreateUserHandler : IRequestHandler<CreateUser, Response>
    {
        private readonly IUserRepository _repository;

        public CreateUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var existsUser = await _repository.ExistsUserAsync(request.Email);

            if (existsUser)
            {
                response.AddNotification(new Notification("user", "Jà existe um usuário com esse e-mail"));
                return response;
            }

            var user = new User(request.Name, request.Email, request.Password);

            if (user.Invalid)
            {
                response.AddNotifications(user.Notifications);
                return response;
            }

            await _repository.SaveAsync(user);

            response.AddValue(new
            {
                user.Id,
                user.Name,
                user.Email
            });

            return response;
        }
    }
}