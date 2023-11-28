namespace FairPlayScheduler.Api.Model
{
    public class Responsibility
    {
        public long PlayerTaskId { get; set; }
        public string? CardName { get; set; }
        public string? Suit { get; set; }
        public string? TaskType { get; set; }
        public string? Requirement { get; set; }
        public Cadence Cadence { get; set; }
        public string? MinimumStandard { get; set; }
        public string? CronSchedule { get; set; }
        public string? When { get; set; }
        public string? Notes { get; set; }
        public long? CompletedTaskId { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool MarkAsComplete { get; set; }
    }
}
