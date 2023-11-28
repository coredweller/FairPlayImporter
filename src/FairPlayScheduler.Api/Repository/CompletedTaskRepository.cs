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

        public async Task<IList<CompletedTask>> GetCompletedTasksAsync(long userId, int days)
        {
            var tasks = new List<CompletedTask>();
            var sql = $@"
                    SELECT pt.Id as 'PlayerTaskId', uc.CardName, uc.Suit, pt.TaskType,
                    pt.Requirement, pt.Cadence, pt.MinimumStandard, 
                    ts.CronSchedule, ts.Notes as 'When', pt.Notes
                    FROM CompletedTask ct
                    JOIN PlayerTask pt ON ct.PlayerTaskId = pt.Id
                    JOIN [UserCard] uc ON uc.Id = pt.CardId
                    LEFT JOIN TaskSchedule ts ON pt.Id = ts.PlayerTaskId
                    WHERE uc.UserId = {userId} AND ct.CompletedDate >= dateadd(day,-{days},ct.CompletedDate)
                    ORDER BY pt.Id ASC;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var completedTasks = await connection.QueryAsync<CompletedTask>(sql);
                tasks = completedTasks.ToList();
            }

            return tasks;
        }
    }
}
