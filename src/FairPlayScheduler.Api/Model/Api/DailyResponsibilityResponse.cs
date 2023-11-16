namespace FairPlayScheduler.Api.Model.Api
{
    public class DailyResponsibilityResponse
    {
        public DateTime Date { get; set; }
        public IList<ResponsibilityResponse> Responsibilities { get; set; } = new List<ResponsibilityResponse>();
    }
}
