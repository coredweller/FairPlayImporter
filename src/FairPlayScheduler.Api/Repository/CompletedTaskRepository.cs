using Dapper;
using FairPlayScheduler.Api.Configuration;
using FairPlayScheduler.Api.Model;
using System.Data.SqlClient;

namespace FairPlayScheduler.Api.Repository
{
    public class CompletedTaskRepository : ICompletedTaskRepository
    {
        private string _connectionString;

        public CompletedTaskRepository(IDatabaseConfig config)
        {
            _connectionString = config.ConnectionString ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
        }

        public async Task<CompletedTask> InsertCompletedTask(CompletedTask task)
        {
            var sql = @"
            INSERT INTO [CompletedTask] (PlayerTaskId, AssignedDate, CompletedDate, Notes)
            OUTPUT INSERTED.Id
            VALUES (@PlayerTaskId, @AssignedDate, @CompletedDate, @Notes);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                task.Id = await connection.QuerySingleAsync<long>(sql, new { PlayerTaskId = task.PlayerTaskId, AssignedDate = task.AssignedDate, CompletedDate = task.CompletedDate, Notes = task.Notes });
            }

            return task;
        }
    }
}
