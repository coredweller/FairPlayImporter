using Dapper;
using FairPlayImporter.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FairPlayImporter.Repository
{
    public class UserRepository : IUserRepo
    {
        private string _connectionString;
        public UserRepository(IConfiguration configuration)  
        {  
            _connectionString = configuration?.GetConnectionString("FairPlayDatabaseContext") ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");  
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
