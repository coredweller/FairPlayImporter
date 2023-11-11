namespace FairPlayScheduler.Api.Model.Api
{
    public class DailyResponsibilityModel
    {
        public DateTime Date { get; set; }
        public IList<ResponsibilityModel> Responsibilities { get; set; } = new List<ResponsibilityModel>();
    }
}
