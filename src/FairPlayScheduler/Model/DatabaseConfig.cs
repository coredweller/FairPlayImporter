namespace FairPlayScheduler.Model
{
    public interface IDatabaseConfig
    {
        public string? ConnectionString { get; set; }
    }

    public class DatabaseConfig : IDatabaseConfig
    {
        public string? ConnectionString { get; set; }
    }
}
