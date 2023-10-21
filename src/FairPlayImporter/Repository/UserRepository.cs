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

        public async Task<List<User>> GetUsersByName(string name)
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

        public async Task<User> CreateUser(string name)
        {
            var sql = @"
                INSERT INTO [User] (Name, CreatedDate)
                OUTPUT INSERTED.Id
                VALUES (@Name, @CreatedDate);";
            var user = new User(name);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                user.Id = await connection.QuerySingleAsync<long>(sql, new { Name = name, CreatedDate = DateTime.UtcNow });
            }

            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            var sql = @"
                UPDATE [User]
                SET UpdatedDate = @UpdatedDate
                WHERE Id = @UserId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { UpdatedDate = user.UpdatedDate, UserId = user.Id });
            }

            return user;
        }
    }
}
