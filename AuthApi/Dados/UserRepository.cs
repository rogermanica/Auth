using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Dominio;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace AuthApi.Dados
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration Configuration { get; }
        private readonly string Conn;

        public UserRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            Conn = Configuration.GetConnectionString("CONNNECTION");
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            string sql = @"SELECT 
                            Nome,
                            Sobrenome,
                            Email,
                            Password
                        FROM 
                            user
                        WHERE 
                            Email = @email";

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { email }, null, null, CommandType.Text);
                
                if(user != null && user.Password.Equals(password))
                {
                    return user;
                }

                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            string sql = @"SELECT Name, Email
                        FROM 
                            users";

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                var users = await connection.QueryAsync<User>(sql, null, null, null, CommandType.Text);
                
                return users.AsList();
            }
        }

        public async Task<User> GetAsync(int id)
        {
            string sql = @"SELECT Name, Email
                        FROM 
                            users
                        WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { id }, null, null, CommandType.Text);
                
                return user;
            }
        }

        public async Task<User> GetAsync(string email)
        {
            string sql = @"SELECT Name, Email
                        FROM 
                            users
                        WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { email }, null, null, CommandType.Text);
                
                return user;
            }
        }

        public async Task<bool> ExistsUserAsync(string email)
        {
            string sql = @"SELECT Name, Email
                        FROM 
                            users
                        WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { email }, null, null, CommandType.Text);
                
                if(user != null)
                    return true;

                return false;
            }
        }

        public async Task<bool> SaveAsync(User user)
        {

            string sql = @" UPDATE users 
                            SET
                            	Name = @name,
                                Password = @password
                            WHERE Id = @id";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Name", user.Name);
            param.Add("Password", user.Password);
            param.Add("Id", user.Id);
            
            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                return (await connection.ExecuteAsync(sql, param, null, null, CommandType.Text)) > 0;
            }
        }

        public async Task<bool> RemoveAsync(User user)
        {

            string sql = @" DELETE FROM users WHERE Id = @id";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", user.Id);
            
            using (SqlConnection connection = new SqlConnection(Conn))
            {
                await connection.OpenAsync();
                return (await connection.ExecuteAsync(sql, param, null, null, CommandType.Text)) > 0;
            }
        }

    }
}
