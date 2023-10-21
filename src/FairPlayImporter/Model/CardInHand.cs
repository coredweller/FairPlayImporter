namespace FairPlayImporter.Model
{
    public class CardInHand
    {
        public CardInHand(UserCard userCard, PlayerTask playerTask, TaskSchedule? schedule)
        {
            UserCard = userCard;
            Task = playerTask;
            Schedule = schedule;
        }

        public UserCard UserCard { get; set; }
        public PlayerTask Task { get; set; }
        public TaskSchedule? Schedule { get; set; }
    }
}
