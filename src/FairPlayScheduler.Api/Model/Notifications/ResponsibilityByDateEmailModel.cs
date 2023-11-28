namespace FairPlayScheduler.Api.Model.Notifications
{
    public class ResponsibilityByDateEmailModel
    {
        public DateTime Date { get; set; }
        public IList<ResponsibilityEmailModel> Responsibilities { get; set; }
    }
}
