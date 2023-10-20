namespace FairPlayImporter.Model
{
    public class User
    {
        public User(string name)
        {
            Name = name;
            CreatedDate = DateTime.UtcNow;
        }

        public User()
        {
            
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get ; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
