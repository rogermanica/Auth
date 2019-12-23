using System.Threading;
using System.Threading.Tasks;
using AuthApi.Dados;
using AuthApi.Requests;
using AuthApi.Util;
using Flunt.Notifications;
using MediatR;

namespace AuthApi.Handler
{
    public class RemoveUserHandler : IRequestHandler<RemoveAccount, Response>
    {
        private readonly IUserRepository _repository;

        public RemoveUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(RemoveAccount request, CancellationToken cancellationToken)
        {
            var response = new Response();
            var user = await _repository.GetAsync(request.Id);

            if (user == null)
            {
                response.AddNotification(new Notification("user", "Usuário não encontrado"));
                return response;
            }

            await _repository.RemoveAsync(user);

            return response;
        }
    }
}