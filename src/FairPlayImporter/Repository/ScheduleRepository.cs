using Dapper;
using FairPlayImporter.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FairPlayImporter.Repository
{
    public interface IScheduleRepo
    {
        Task<TaskSchedule> CreateTaskSchedule(TaskSchedule schedule);
    }

    public class ScheduleRepository : IScheduleRepo
    {
        private string _connectionString;
        public ScheduleRepository(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("FairPlayDatabaseContext") ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
        }

        public async Task<TaskSchedule> CreateTaskSchedule(TaskSchedule schedule)
        {
            var sql = @"
            INSERT INTO [TaskSchedule] (PlayerTaskId, CronSchedule, Notes)
            OUTPUT INSERTED.Id
            VALUES (@PlayerTaskId, @CronSchedule, @Notes);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                schedule.Id = await connection.QuerySingleAsync<long>(sql, new { PlayerTaskId = schedule.PlayerTaskId, CronSchedule = schedule.CronSchedule, Notes = schedule.Notes });
            }

            return schedule;
        }
    }
}
