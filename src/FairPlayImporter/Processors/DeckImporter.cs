using FairPlayImporter.Model;
using FairPlayImporter.Repository;

namespace FairPlayImporter.Processors
{
    public interface IImportDecks
    {
        Task<ImportResults> Import(string userName, string fileLocation);
    }

    public class DeckImporter : IImportDecks
    {
        private readonly IUserRepo _userRepo;
        private readonly ICalculateSchedules _scheduler;

        public DeckImporter(IUserRepo userRepo, ICalculateSchedules scheduler)
        {
            _userRepo = userRepo;
            _scheduler = scheduler;
        }

        public async Task<ImportResults> Import(string userName, string fileLocation)
        {
            var result = new ImportResults();

            var currentUser = await GetUser(userName);

            var tasks = await ParseCsv(fileLocation, currentUser.Id);



            return result;
        }

        private async Task<List<PlayerTask>> ParseCsv(string fileLocation, long userId)
        {
            List<PlayerTask> playerTasks = new List<PlayerTask>();
            using (var reader = new StreamReader(fileLocation))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var userCard = new UserCard { CardName = values[0], Suit = values[1], UserId = userId };

                    Cadence cadence;
                    if(!Enum.TryParse(values[4], true, out cadence))
                    {
                        throw new ArgumentException($"Cadence of {values[4]} is invalid!");
                    }

                    var playerTask = new PlayerTask { TaskType = values[2], Requirement = values[3], CadenceId = (short)cadence, MinimumStandard = values[6], Notes = values[7] };

                    var when = values[5];
                    playerTasks.Add(playerTask);
                    if (when != null)
                    {
                        var schedule = _scheduler.GenerateSchedule(playerTask.Id, when, cadence, when);
                        // TODO: When? is used to create TaskSchedule
                        // Save schedule
                    }
                    
                    //TODO: LEFT OFF HERE
                        // Save all things to DB

                }
            }

            return playerTasks;
        }

        private async Task<User> GetUser(string userName)
        {
            var usersByName = await _userRepo.GetUsersByName(userName);
            var users = usersByName.OrderByDescending(u => u.UpdatedDate.HasValue ? u.UpdatedDate : u.CreatedDate);
            var currentUser = new User();
            switch (users.Any())
            {
                case true:
                    currentUser = users.First();
                    currentUser.UpdatedDate = DateTime.UtcNow;
                    await _userRepo.UpdateUser(currentUser);
                    break;
                case false:
                    currentUser = await _userRepo.CreateUser(userName);
                    break;
            }

            return currentUser;
        }
    }
}
