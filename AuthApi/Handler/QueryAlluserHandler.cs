using AuthApi.Dados;
using AuthApi.Requests;
using AuthApi.Util;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Handler
{
    public class QueryAllUsersHandler : IRequestHandler<QueryAllUsers, Response>
    {
        private readonly IUserRepository _repository;

        public QueryAllUsersHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(QueryAllUsers request, CancellationToken cancellationToken)
        {
            var response = new Response();
            var users = await _repository.GetUsersAsync();
                
            var result = users.Select(a=> new
            {
                a.Id,
                a.Name,
                a.Email,
            });

            response.AddValue(result);

            return response;
        }
    }
}