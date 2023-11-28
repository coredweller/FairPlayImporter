namespace FairPlayScheduler.Api.Model
{
    public class ResponsibilityByDay
    {
        public DateTime Date { get; set; }
        public IList<Responsibility> Responsibilities { get; set; } = new List<Responsibility>();
    }
}
