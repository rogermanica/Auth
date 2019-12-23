using System.Collections.Generic;
using System.Threading.Tasks;
using AuthApi.Dominio;

namespace AuthApi.Dados
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string email, string password);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetAsync(int id);

        Task<User> GetAsync(string email);

        Task<bool> ExistsUserAsync(string email);

        Task<bool> SaveAsync(User user);

        Task<bool> RemoveAsync(User user);
    }
}