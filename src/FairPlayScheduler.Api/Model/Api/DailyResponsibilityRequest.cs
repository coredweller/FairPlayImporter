namespace FairPlayScheduler.Api.Model.Api
{
    public class DailyResponsibilityRequest
    {
        public DateTime Date { get; set; }
        public IList<ResponsibilityRequest> Responsibilities { get; set; } = new List<ResponsibilityRequest>();
    }
}
