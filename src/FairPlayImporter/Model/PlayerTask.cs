namespace FairPlayImporter.Model
{
    public class PlayerTask
    {
        public long Id { get; set; }
        public long CardId { get; set; }
        public string? TaskType { get; set; }
        public string? Requirement { get; set; }
        public short CadenceId { get; set; }
        public string? MinimumStandard { get; set; }
        public string? Notes { get; set; }
    }
}
