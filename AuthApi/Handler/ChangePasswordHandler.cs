using System.Threading;
using System.Threading.Tasks;
using AuthApi.Dados;
using AuthApi.Requests;
using AuthApi.Util;
using MediatR;

namespace AuthApi.Handler
{
    public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPassword, Response>
    {
        private readonly IUserRepository _repository;
        private readonly AuthenticatedUser _user;

        public ChangeUserPasswordHandler(IUserRepository repository, AuthenticatedUser user)
        {
            _repository = repository;
            _user = user;
        }

        public async Task<Response> Handle(ChangeUserPassword request, CancellationToken cancellationToken)
        {
            var response = new Response();

            var user = await _repository.GetAsync(_user.Email);

            user.ChangePassword(request.NewPassword, request.NewPasswordConfirmation);

            if (user.Invalid)
            {
                response.AddNotifications(user.Notifications);
                return response;
            }

            await _repository.SaveAsync(user);

            return response;
        }
    }
}