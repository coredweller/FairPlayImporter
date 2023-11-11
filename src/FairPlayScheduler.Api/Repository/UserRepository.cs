using Dapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FairPlayScheduler.Api.Repository
{
    public class UserRepository : IUserRepo
    {
        private string _connectionString;
        public UserRepository(IDatabaseConfig configuration)
        {
            _connectionString = configuration.ConnectionString ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
        }

        public async Task<IList<User>> GetUsersByName(string name)
        {
            var users = new List<User>();
            var sql = $"SELECT * FROM [User] WHERE Name = '{name}';";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var usersEnum = await connection.QueryAsync<User>(sql);
                users = usersEnum.ToList();
            }

            return users;
        }
    }
}
