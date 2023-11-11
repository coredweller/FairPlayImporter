namespace FairPlayScheduler.Api.Model
{
    public class CompletedTask
    {
        public long Id { get; set; }
        public long PlayerTaskId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime CompletedDate { get; set;}
        public string? Notes { get; set; }
    }
}
