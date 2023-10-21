using Dapper;
using FairPlayImporter.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace FairPlayImporter.Repository
{
    public interface ICardRepo
    {
        Task<UserCard> CreateUserCard(UserCard card);
        Task<UserCard?> GetUserCardByCardName(UserCard card);
        Task<UserCard> GetOrCreateUserCard(UserCard card);
        Task<PlayerTask> CreatePlayerTask(PlayerTask task);
    }

    public class CardRepository : ICardRepo
    {
        private string _connectionString;
        public CardRepository(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("FairPlayDatabaseContext") ?? throw new ArgumentException("NO CONFIGURATION FOR CONNECTION STRINGS!!");
        }

        public async Task<UserCard> GetOrCreateUserCard(UserCard card)
        {
            var existingCard = await GetUserCardByCardName(card);
            if (existingCard != null) return existingCard;

            return await CreateUserCard(card);
        }

        public async Task<UserCard> CreateUserCard(UserCard card)
        {
            var sql = @"
            INSERT INTO [UserCard] (CardName, Suit, UserId)
            OUTPUT INSERTED.Id
            VALUES (@CardName, @Suit, @UserId);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                card.Id = await connection.QuerySingleAsync<long>(sql, new { CardName = card.CardName, Suit = card.Suit, UserId = card.UserId });
            }

            return card;
        }

        public async Task<UserCard?> GetUserCardByCardName(UserCard card)
        {
            var sql = $"SELECT * FROM [UserCard] WHERE CardName = '{card.CardName}' and UserId = {card.UserId};";
            UserCard? userCard;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                userCard = (await connection.QueryAsync<UserCard>(sql)).FirstOrDefault();
            }

            return userCard;
        }

        public async Task<PlayerTask> CreatePlayerTask(PlayerTask task)
        {
            var sql = @"
            INSERT INTO [PlayerTask] (CardId, TaskType, Requirement, Cadence, MinimumStandard, Notes)
            OUTPUT INSERTED.Id
            VALUES (@CardId, @TaskType, @Requirement, @Cadence, @MinimumStandard, @Notes);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                task.Id = await connection.QuerySingleAsync<long>(sql, 
                    new { CardId = task.CardId, TaskType = task.TaskType, Requirement = task.Requirement,
                          Cadence = task.CadenceId, MinimumStandard = task.MinimumStandard, Notes = task.Notes });
            }

            return task;
        }
    }
}
