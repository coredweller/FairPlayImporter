namespace FairPlayScheduler.Api.Model.Api
{
    public class CompletedTaskResponse
    {
        public long Id { get; set; }
        public long PlayerTaskId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public string? Notes { get; set; }
    }
}
