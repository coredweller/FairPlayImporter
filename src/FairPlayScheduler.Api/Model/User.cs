namespace FairPlayScheduler.Api.Model
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get ; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public string? Email { get; set; }
    }
}
