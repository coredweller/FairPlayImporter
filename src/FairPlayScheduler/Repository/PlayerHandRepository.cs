using Dapper;
using FairPlayScheduler.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace FairPlayScheduler.Repository
{
    public interface IPlayerHandRepo
    {
        Task<IList<Responsibility>> GetResponsibilitiesAsync(long userId);
    }

    public class PlayerHandRepository : IPlayerHandRepo
    {
        private string _connectionString;
        public PlayerHandRepository(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("FairPlayDatabaseContext") ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
        }

        public async Task<IList<Responsibility>> GetResponsibilitiesAsync(long userId)
        {
            var userResponsibilities = new List<Responsibility>();
            var sql = $@"
                    SELECT pt.Id as 'PlayerTaskId', uc.CardName, uc.Suit, pt.TaskType,
                    pt.Requirement, pt.Cadence as 'CadenceId', pt.MinimumStandard, 
                    ts.CronSchedule, ts.Notes as 'When', pt.Notes
                    FROM PlayerTask pt 
                    JOIN [UserCard] uc ON uc.Id = pt.CardId
                    LEFT JOIN TaskSchedule ts ON pt.Id = ts.PlayerTaskId
                    WHERE uc.UserId = {userId}
                    ORDER BY pt.Id ASC;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var responsibilities = await connection.QueryAsync<Responsibility>(sql);
                userResponsibilities = responsibilities.ToList();
            }

            return userResponsibilities;
        }
    }
}
