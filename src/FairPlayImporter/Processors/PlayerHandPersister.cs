using FairPlayImporter.Model;
using FairPlayImporter.Repository;

namespace FairPlayImporter.Processors
{
    public interface IPersistPlayerHands
    {
        PlayerHand SavePlayerHand(PlayerHand hand);
    }

    public class PlayerHandPersister : IPersistPlayerHands
    {
        private readonly ICardRepo _cardRepo;
        private readonly IScheduleRepo _scheduleRepo;

        public PlayerHandPersister(ICardRepo cardRepo, IScheduleRepo scheduleRepo)
        {
            _cardRepo = cardRepo;
            _scheduleRepo = scheduleRepo;
        }

        public PlayerHand SavePlayerHand(PlayerHand hand)
        {
            var cardsInHand = hand.Cards.Select(async card =>
            {
                try
                {
                    var savedCard = await _cardRepo.GetOrCreateUserCard(card.UserCard);
                    card.Task.CardId = savedCard.Id;
                    var savedTask = await _cardRepo.CreatePlayerTask(card.Task);
                    if (card.Schedule != null)
                    {
                        card.Schedule.PlayerTaskId = savedTask.Id;
                        var savedSchedule = await _scheduleRepo.CreateTaskSchedule(card.Schedule);
                    }
                    return card;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Invalid saving of card: {card.UserCard.CardName} with error: {ex}");
                }
            }).Select(t => t.Result).ToList(); //run it synchronously so UserCards are not duplicated
            return new PlayerHand(cardsInHand);
        }
    }
}
