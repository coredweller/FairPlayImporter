namespace FairPlayScheduler.Api.Model.Api
{
    public class SendResponsibilitiesEmailRequest
    {
        public string SenderUserName { get; set; }
        public string SenderPassword { get; set; }

        public DailyResponsibilityRequest ResponsibilitySet { get; set; } = new DailyResponsibilityRequest();
    }
}
