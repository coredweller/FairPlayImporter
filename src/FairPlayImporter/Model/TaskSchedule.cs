namespace FairPlayImporter.Model
{
    public class TaskSchedule
    {
        public long Id { get; set; }
        public long PlayerTaskId { get; set; }
        public string? CronSchedule { get; set; }
        public string? Notes { get; set; }
    }
}
