using Flunt.Notifications;
using MediatR;

namespace AuthApi.Util
{
    public abstract class Request<TResponse> : Notifiable, IRequest<TResponse>
    {

    }
}