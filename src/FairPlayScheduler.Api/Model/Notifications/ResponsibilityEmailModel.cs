namespace FairPlayScheduler.Api.Model.Notifications
{
    public class ResponsibilityEmailModel
    {
        public long PlayerTaskId { get; set; }
        public string? CardName { get; set; }
        public string? Suit { get; set; }
        public string? TaskType { get; set; }
        public string? Requirement { get; set; }
        public string? CadenceName { get; set; }
        public bool HasMinimumStandard { get { return !string.IsNullOrWhiteSpace(MinimumStandard); } }
        public string? MinimumStandard { get; set; }
        public string? Schedule { get; set; }
        public bool HasWhen { get { return !string.IsNullOrWhiteSpace(When); } }
        public string? When { get; set; }
        public bool HasNotes { get { return !string.IsNullOrWhiteSpace(Notes); } }
        public string? Notes { get; set; }
        public bool IsCompleted { get; set; }
    }
}
