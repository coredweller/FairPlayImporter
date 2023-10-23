namespace FairPlayScheduler.Model
{
    public class ResponsibilityByDay
    {
        public ResponsibilityByDay(DateTime date)
        {
            Date = date;
            Responsibilities = new List<Responsibility>();
        }
        public DateTime Date { get; set; }
        public IList<Responsibility> Responsibilities { get; set; }
    }
}
