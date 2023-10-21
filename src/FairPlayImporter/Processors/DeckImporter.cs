using FairPlayImporter.Model;
using FairPlayImporter.Repository;

namespace FairPlayImporter.Processors
{
    public interface IImportDecks
    {
        Task<PlayerHand> Import(string userName, string fileLocation);
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

        public async Task<PlayerHand> Import(string userName, string fileLocation)
        {
            var currentUser = await GetUser(userName);
            var hand = ParseCsv(fileLocation, currentUser.Id);
            return hand;
        }

        private PlayerHand ParseCsv(string fileLocation, long userId)
        {
            var firstLine = true;
            List<CardInHand> cardsInHand = new List<CardInHand>();
            using (var reader = new StreamReader(fileLocation))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if(firstLine)
                    {
                        line = reader.ReadLine();
                        firstLine = false;
                    }

                    var values = line.Split(';');
                    var userCard = new UserCard { CardName = values[0], Suit = values[1], UserId = userId };

                    Cadence cadence;
                    if(!Enum.TryParse(values[4], true, out cadence))
                    {
                        throw new ArgumentException($"Cadence of {values[4]} is invalid!");
                    }

                    var playerTask = new PlayerTask { TaskType = values[2], Requirement = values[3], CadenceId = (short)cadence, MinimumStandard = values[6], Notes = values[7] };
                    TaskSchedule? schedule = null;
                    var when = values[5];
                    if (!string.IsNullOrWhiteSpace(when))
                    {
                        schedule = _scheduler.GenerateSchedule(userCard.CardName, playerTask.Id, when, cadence, when);
                    }

                    cardsInHand.Add(new CardInHand(userCard, playerTask, schedule));
                }
            }

            return new PlayerHand(cardsInHand);
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
