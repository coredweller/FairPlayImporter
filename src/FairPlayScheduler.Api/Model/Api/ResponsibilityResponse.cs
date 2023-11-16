namespace FairPlayScheduler.Api.Model.Api
{
    public class ResponsibilityResponse
    {
        public long PlayerTaskId { get; set; }
        public string? CardName { get; set; }
        public string? Suit { get; set; }
        public string? TaskType { get; set; }
        public string? Requirement { get; set; }
        public string? Cadence { get; set; }
        public string? MinimumStandard { get; set; }
        public string? Schedule { get; set; }
        public string? When { get; set; }
        public string? Notes { get; set; }
    }
}
